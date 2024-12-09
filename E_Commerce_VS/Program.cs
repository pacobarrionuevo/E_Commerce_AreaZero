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

            StripeConfiguration.ApiKey = builder.Configuration.GetSection(Settings.SECTION_NAME).Get<Settings>()?.StripeSecret;

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
            builder.Services.AddScoped<RepositorioCarrito>();

            builder.Services.AddScoped<Services.ProductService>();
            builder.Services.AddScoped<Services.ReviewService>();
            builder.Services.AddScoped<Services.SmartSearchService>();

            // A adimos los mappers como Transient
            builder.Services.AddScoped<ProductoMapper>();
            builder.Services.AddScoped<ReviewMapper>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Configuracion de MLModel para las reseñas
            builder.Services.AddPredictionEnginePool<ModelInput, ModelOutput>().FromFile(modelPath);

            // Configuraci n de CORS
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            builder.Services.AddAuthentication().AddJwtBearer(options =>
            {
                Settings settings = builder.Configuration.GetSection(Settings.SECTION_NAME).Get<Settings>();
                string key = settings.JwtKey;

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    //Pagina 94 del PDF de Jose
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
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

            app.UseCors();

            // Autenticacion y Autorizacion
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            await InitDatabaseAsync(app.Services);

            InitStripe(app.Services);

            app.Run();
        }

        //Métodos de Jose para iniciar la base de datos y el stripe
        static async Task InitDatabaseAsync(IServiceProvider serviceProvider)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            using ProyectoDbContext dbContext = scope.ServiceProvider.GetService<ProyectoDbContext>();

            // Si no existe la base de datos entonces la creamos y ejecutamos el seeder
            if (dbContext.Database.EnsureCreated())
            {
                Seeder seeder = new Seeder(dbContext);
                await seeder.SeedAsync();
            }
        }

        static void InitStripe(IServiceProvider serviceProvider)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            IOptions<Settings> options = scope.ServiceProvider.GetService<IOptions<Settings>>();

            // Ponemos nuestro secret key (se consulta en el dashboard => desarrolladores)
            StripeConfiguration.ApiKey = options.Value.StripeSecret;
        }

    }
}