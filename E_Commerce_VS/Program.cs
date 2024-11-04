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
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

            // Aï¿½adimos los mappers como Transient
            builder.Services.AddScoped<ProductoMapper>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<ProyectoDbContext>();

<<<<<<< HEAD
            // Configuración de autenticación JWT
            builder.Services.AddAuthentication(options =>
=======
            // Configuraciï¿½n de CORS
            builder.Services.AddCors(options =>
>>>>>>> origin/gonza
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Clave para la firma del JWT
                string key = Environment.GetEnvironmentVariable("JWT_KEY") ?? "aljdvb##@coienwe82784f8fnuioecwcq2";
                Console.WriteLine($"JWT_KEY: {key}");

                options.TokenValidationParameters = new TokenValidationParameters()
                {
<<<<<<< HEAD
=======
                    //Pï¿½gina 94 del PDF de Jose
>>>>>>> origin/gonza
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddCors(options =>
                {
                    options.AddDefaultPolicy(builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
                });
            }

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            // Autenticaciï¿½n y Autorizaciï¿½n
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
