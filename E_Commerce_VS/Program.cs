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

namespace E_Commerce_VS
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<Settings>(builder.Configuration.GetSection(Settings.SECTION_NAME));

            builder.Services.Configure<Settings>(builder.Configuration.GetSection(Settings.SECTION_NAME));

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddScoped<ProyectoDbContext>();
            builder.Services.AddScoped<UnitOfWork>();
            builder.Services.AddScoped<RepositorioProducto>();

            builder.Services.AddScoped<Services.ProductService>();

            // A�adimos los mappers como Transient
            builder.Services.AddScoped<ProductoMapper>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<ProyectoDbContext>();

            // Configuraci�n de CORS
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
                string Key = Environment.GetEnvironmentVariable("JWT_KEY");
                Console.WriteLine($"JWT_KEY: {Key}");

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    //P�gina 94 del PDF de Jose
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

            // Autenticaci�n y Autorizaci�n
            app.UseAuthentication();
            app.UseAuthorization();

            // Habilitar CORS
            app.UseCors();

            app.UseStaticFiles();
            app.MapControllers();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                ProyectoDbContext _dbContext = scope.ServiceProvider.GetService<ProyectoDbContext>();
                _dbContext.Database.EnsureCreated();
            }

            // Habilitar CORS
            app.UseCors();

            app.UseStaticFiles();
            app.MapControllers();

            InitStripe(app.Services);

            using (IServiceScope scope = app.Services.CreateScope())
            {
                ProyectoDbContext _dbContext = scope.ServiceProvider.GetService<ProyectoDbContext>();
                _dbContext.Database.EnsureCreated();
            }
            app.Run();
        }

        
    }
}
