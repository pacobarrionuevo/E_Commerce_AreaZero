using E_Commerce_VS.Models.Database;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Stripe;
using Microsoft.Extensions.Options;

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

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<ProyectoDbContext>();

            // Configuración de CORS
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

            // Autenticación y Autorización
            app.UseAuthentication();
            app.UseAuthorization();

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

        static void InitStripe(IServiceProvider serviceProvider)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            IOptions<Settings> options = scope.ServiceProvider.GetService<IOptions<Settings>>();
            StripeConfiguration.ApiKey = options.Value.StripeSecret;
        }
    }
}
