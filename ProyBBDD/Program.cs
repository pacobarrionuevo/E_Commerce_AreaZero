
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace E_Commerce_VS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            }
            
            );
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    //antes del f "" 8 ya hay 24 caracteres (256 bits)
                    //Debería establecerse una variable de entorno

                    string key = "aljdvb##@coienwe82784f8fnuioecwcq2";

                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        //Página 94 del PDF de Jose

                        ValidateIssuer = false,

                        ValidateAudience = false,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };

                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapControllers();

            app.UseAuthentication();

            app.UseAuthorization();

            app.Run();
        }
    }
}
