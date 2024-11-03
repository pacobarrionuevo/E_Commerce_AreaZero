<<<<<<< HEAD
=======
using E_Commerce_VS.Models.Database;
>>>>>>> origin/development
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

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddScoped<ProyectoDbContext>();
            builder.Services.AddScoped<UnitOfWork>();
            builder.Services.AddScoped<RepositorioProducto>();

            builder.Services.AddScoped<Services.ProductService>();

            // Añadimos los mappers como Transient
            builder.Services.AddScoped<ProductoMapper>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

<<<<<<< HEAD
            builder.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    //antes del f "" 8 ya hay 24 caracteres (256 bits)
                    //Deberia establecerse una variable de entorno

                    string key = "aljdvb##@coienwe82784f8fnuioecwcq2";

                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        //Pagina 94 del PDF de Jose

                        ValidateIssuer = false,

                        ValidateAudience = false,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };

                });

            if (builder.Environment.IsDevelopment())
=======
            // Configuración de CORS
            builder.Services.AddCors(options =>
>>>>>>> origin/development
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
                    //Página 94 del PDF de Jose
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

            // Autenticación y Autorización
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

            app.Run();
        }

        
    }
}
