using E_Commerce_VS.Models.Database;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using E_Commerce_VS.Models.Database.Repositories;
using E_Commerce_VS.Models.Mapper;
using Microsoft.EntityFrameworkCore;
using E_Commerce_VS.Services;
using E_Commerce_VS.Models.Database.Entidades;
using Microsoft.Extensions.ML;
using Stripe;

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

            // Configuración de MLModel para las reseñas
            builder.Services.AddPredictionEnginePool<ModelInput, ModelOutput>().FromFile(modelPath);

            // Configuración de CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
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
            app.UseCors("AngularApp");


            // Configuramos CORS para que acepte cualquier petición de cualquier origen (no es seguro)
            app.UseCors("AllowAllOrigins");

            // Autenticación y Autorización
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // Inicialización de la base de datos
            await InitDatabaseAsync(app.Services);

            // Inicialización de Stripe
            InitStripe(app.Services);

            // Empezamos a atender a las peticiones de nuestro servidor 
            await app.RunAsync();
        }

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
