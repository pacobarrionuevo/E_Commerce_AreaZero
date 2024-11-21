using E_Commerce_VS.Models.Database;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Stripe;
using Microsoft.Extensions.Options;
using E_Commerce_VS.Models.Database.Repositories;
using E_Commerce_VS.Models.Mapper;
using Microsoft.EntityFrameworkCore;
using E_Commerce_VS.Services;
using E_Commerce_VS.Models.Database.Entidades;
using Microsoft.Extensions.ML;

namespace E_Commerce_VS
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string modelPath = Path.Combine(Environment.CurrentDirectory, "MLModel_AreaZero.mlnet");

            Directory.SetCurrentDirectory(AppContext.BaseDirectory);

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<Settings>(builder.Configuration.GetSection(Settings.SECTION_NAME));

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddScoped<ProyectoDbContext>();
            builder.Services.AddScoped<UnitOfWork>();
            builder.Services.AddScoped<RepositorioProducto>();
            builder.Services.AddScoped<RepositorioReview>();

            builder.Services.AddScoped<Services.ProductService>();
            builder.Services.AddScoped<Services.ReviewService>();
            builder.Services.AddScoped<Services.SmartSearchService>();

            // Añadimos los mappers como Transient
            builder.Services.AddScoped<ProductoMapper>();
            builder.Services.AddScoped<ReviewMapper>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<ProyectoDbContext>();

            // Configuracion de MLModel para las reseñas
            builder.Services.AddPredictionEnginePool<ModelInput, ModelOutput>().FromFile(modelPath);

            // Configuración de CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.Services.AddAuthentication().AddJwtBearer(options =>
            {
                string Key = Environment.GetEnvironmentVariable("JWT_KEY");
                Console.WriteLine($"JWT_KEY: {Key}");

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key))
                };
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Autenticacion y Autorizacion
            app.UseAuthentication();
            app.UseAuthorization();

            // Configuramos CORS para que acepte cualquier petición de cualquier origen (no es seguro)
            app.UseCors("AllowAllOrigins");

            app.MapControllers();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                ProyectoDbContext _dbContext = scope.ServiceProvider.GetService<ProyectoDbContext>();
                _dbContext.Database.EnsureCreated();

                var seeder = new Seeder(_dbContext);
                await seeder.SeedAsync();
            }

            app.Run();
        }
    }
}
