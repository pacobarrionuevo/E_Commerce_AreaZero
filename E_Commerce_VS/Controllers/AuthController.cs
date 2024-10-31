using E_Commerce_VS.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using E_Commerce_VS.Database;

namespace E_Commerce_VS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly TokenValidationParameters _tokenParameters;

        public AuthController(IOptionsMonitor<JwtBearerOptions> jwtOptions)
        {
            _tokenParameters = jwtOptions.Get(JwtBearerDefaults.AuthenticationScheme).TokenValidationParameters;
        }


        //Le voy a poner una peticion que me va a devolver un string 
        [Authorize]
        [HttpGet]
        public string GetUserIdentifiedMessage()
        {
            return "Si puedes leer esto es que estás identificado";
        }

        [Authorize(Roles = "admin")]
        [HttpGet("admin")]
        public string GetAdminMessage()
        {
            return "Si puedes leer esto es que eres admin";
        }

        [HttpPost]
        public ActionResult<string> Login([FromBody] LoginDto data)
        {
            if (data.Usuario == "paco" && data.Password == "123456")
            {
                //Autorizado
                //Consulta a la base de datos si está el usuario o no

                //Inferencia de tipo: coge el tipo de la variable y te lo escribe a ti
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Claims = new Dictionary<string, Object>
                    {
                        { "id", 3 },
                        { ClaimTypes.Role, "admin"}
                    },
                    Expires = DateTime.UtcNow.AddYears(3),
                    SigningCredentials = new SigningCredentials(
                        _tokenParameters.IssuerSigningKey,
                        SecurityAlgorithms.HmacSha256Signature)
                };
                //Manejador de Token
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                string stringToken = tokenHandler.WriteToken(token);

                //El Ok() es para que devuelva codigo200 (todo bien)
                return Ok(stringToken);

            }
            else
            {
                return Unauthorized();
            }
        }
    }
}