using E_Commerce_VS.Models.Database.Entidades;
using E_Commerce_VS.Models.Database;
using E_Commerce_VS.Models.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using E_Commerce_VS.Recursos;

namespace E_Commerce_VS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorUsuario : ControllerBase
    {
        private readonly ProyectoDbContext _context;
        private readonly PasswordHelper passwordHelper;
        private readonly TokenValidationParameters _tokenParameters;

        public ControladorUsuario(ProyectoDbContext _dbContext, IOptionsMonitor<JwtBearerOptions> jwtOptions)
        {
            _context = _dbContext;
            _tokenParameters = jwtOptions.Get(JwtBearerDefaults.AuthenticationScheme).TokenValidationParameters;
        }

        // Método para obtener la lista de usuarios
        [HttpGet("userlist")]
        public IEnumerable<UserDto> GetUser()
        {
            return _context.Usuarios.Select(ToDto);
        }

        [HttpPost("Registro")]
        public async Task<IActionResult> Register([FromBody] UserDto usuario)
        {
            if (_context.Usuarios.Any(u => u.Nombre == usuario.Nombre))
            {
                return BadRequest("El nombre del usuario ya está en uso");
            }

            Usuario newUser = new Usuario()
            {
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Password = PasswordHelper.Hash(usuario.Password),
                Direccion = usuario.Direccion,
                EsAdmin = usuario.EsAdmin
            };

            await _context.Usuarios.AddAsync(newUser);
            await _context.SaveChangesAsync();
            UserDto userCreated = ToDto(newUser);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = new Dictionary<string, object>
                {
                    {"id", newUser.UsuarioId},
                    {"Nombre", newUser.Nombre},
                    {"Email", newUser.Email},
                    {"Direccion", newUser.Direccion},
                    {"EsAdmin", newUser.EsAdmin}
                },
                Expires = DateTime.UtcNow.AddDays(5),
                SigningCredentials = new SigningCredentials(
                    _tokenParameters.IssuerSigningKey,
                    SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string accessToken = tokenHandler.WriteToken(token);
            return Ok(new { StringToken = accessToken });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDto userLoginDto)
        {
            var user = _context.Usuarios.FirstOrDefault(u => u.Email == userLoginDto.Email);
            if (user == null)
            {
                return Unauthorized("Usuario no existe");
            }

            var hashedPassword = PasswordHelper.Hash(userLoginDto.Password);
            if (user.Password != hashedPassword)
            {
                return Unauthorized("Contraseña incorrecta");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = new Dictionary<string, object>
                {
                    {"id", user.UsuarioId},
                    {"Nombre", user.Nombre},
                    {"Email", user.Email},
                    {"Direccion", user.Direccion},
                    {"EsAdmin", user.EsAdmin}
                },
                Expires = DateTime.UtcNow.AddDays(5),
                SigningCredentials = new SigningCredentials(
                    _tokenParameters.IssuerSigningKey,
                    SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string accessToken = tokenHandler.WriteToken(token);

            return Ok(new { StringToken = accessToken, user.UsuarioId });
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto userUpdateDto)
        {
            var user = await _context.Usuarios.FindAsync(userUpdateDto.Id);
            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            user.Nombre = userUpdateDto.Nombre;
            user.Email = userUpdateDto.Email;
            user.Direccion = userUpdateDto.Direccion;
            user.EsAdmin = userUpdateDto.EsAdmin;

            _context.Usuarios.Update(user);
            await _context.SaveChangesAsync();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = new Dictionary<string, object>
                {
                    {"id", user.UsuarioId},
                    {"Nombre", user.Nombre},
                    {"Email", user.Email},
                    {"Direccion", user.Direccion},

                    {"EsAdmin", user.EsAdmin}
                },
                Expires = DateTime.UtcNow.AddDays(5),
                SigningCredentials = new SigningCredentials(
                    _tokenParameters.IssuerSigningKey,
                    SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string accessToken = tokenHandler.WriteToken(token);

            return Ok(new { StringToken = accessToken });
        }

        private UserDto ToDto(Usuario user)
        {
            return new UserDto()
            {
                Id = user.UsuarioId.ToString(),
                Nombre = user.Nombre,
                Email = user.Email,
                Password = user.Password,
                Direccion = user.Direccion,
                EsAdmin = user.EsAdmin
            };
        }
    }
}
