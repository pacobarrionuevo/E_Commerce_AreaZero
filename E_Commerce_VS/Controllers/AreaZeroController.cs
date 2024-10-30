using Microsoft.AspNetCore.Mvc;
using E_Commerce_VS.Database.Entidades;
using Microsoft.EntityFrameworkCore;
using E_Commerce_VS.Database;
using E_Commerce_VS.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace E_Commerce_VS.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AreaZeroController : ControllerBase
    {
        private ProyectoDbContext _dbContext;

        public AreaZeroController(ProyectoDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        private readonly TokenValidationParameters _tokenParameters;

        public AreaZeroController(IOptionsMonitor<JwtBearerOptions> jwtOptions)
        {
            _tokenParameters = jwtOptions.Get(JwtBearerDefaults.AuthenticationScheme).TokenValidationParameters;
        }

        //Devolver todos los usuarios
        [HttpGet]
        public IEnumerable<Usuario> GetUsuarios()
        {
            return _dbContext.Usuarios;
        }

        [HttpPost]
        public ActionResult<string> Login([FromBody] LoginDto data)
        {
            if (data.Usuario == "paco" && data.Password == "paCo33")
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
