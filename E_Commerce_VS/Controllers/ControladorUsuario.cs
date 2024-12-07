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

        // Este método nos va a servir para comprobar que los usuarios se añaden
        [HttpGet("userlist")]
        public IEnumerable<UserRegistrarseDto> GetUser()
        {
            return _context.Usuarios.Select(ToDto);
        }

        [HttpPost("Registro")]
        public async Task<IActionResult> Register([FromBody] UserRegistrarseDto usuario)
        {
            // Comprobar con la bbdd
            if (_context.Usuarios.Any(Usuario => Usuario.Nombre == usuario.Nombre))
            {
                return BadRequest("El nombre del usuario ya está en uso");
            }

            Usuario newUser = new Usuario()
            {
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Password = PasswordHelper.Hash(usuario.Password),
                Direccion = usuario.Direccion,
            };

            await _context.Usuarios.AddAsync(newUser);
            await _context.SaveChangesAsync();
            UserRegistrarseDto userCreated = ToDto(newUser);

            // Crear el Token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Datos para autorizar al usuario
                Claims = new Dictionary<string, object>
                {
                    {"id", newUser.UsuarioId},
                    {"Nombre", newUser.Nombre},
                    {"Email", newUser.Email},
                    {"Direccion", newUser.Direccion}
                },
                // Caducidad del Token
                Expires = DateTime.UtcNow.AddDays(5),
                // Clave y algoritmo de firmado
                SigningCredentials = new SigningCredentials(
                    _tokenParameters.IssuerSigningKey,
                    SecurityAlgorithms.HmacSha256Signature)
            };

            // Crear el token y devolverlo al usuario registrado
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string accessToken = tokenHandler.WriteToken(token);
            return Ok(new { StringToken = accessToken });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto userLoginDto)
        {
            foreach (Usuario userList in _context.Usuarios.ToList())
            {
                if (userList.Email == userLoginDto.Email)
                {
                    var result = PasswordHelper.Hash(userLoginDto.Password);
                    if (userList.Password == result)
                    {
                        // Crear el Token
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            // Datos para autorizar al usuario
                            Claims = new Dictionary<string, object>
                            {
                                {"id", userList.UsuarioId},
                                {"Nombre", userList.Nombre},
                                {"Email", userList.Email},
                                {"Direccion", userList.Direccion}
                            },
                            // Caducidad del Token
                            Expires = DateTime.UtcNow.AddDays(5),
                            // Clave y algoritmo de firmado
                            SigningCredentials = new SigningCredentials(
                                _tokenParameters.IssuerSigningKey,
                                SecurityAlgorithms.HmacSha256Signature)
                        };

                        // Crear el token y devolverlo al usuario logueado
                        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                        string accessToken = tokenHandler.WriteToken(token);

                        return Ok(new { StringToken = accessToken, userList.UsuarioId });
                    }
                }
            }
            return Unauthorized("Usuario no existe");
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto userUpdateDto)
        {
            var user = await _context.Usuarios.FindAsync(userUpdateDto.UsuarioId);
            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            user.Nombre = userUpdateDto.Nombre;
            user.Email = userUpdateDto.Email;
            user.Direccion = userUpdateDto.Direccion;

            _context.Usuarios.Update(user);
            await _context.SaveChangesAsync();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = new Dictionary<string, object>
        {
            {"id", user.UsuarioId},
            {"Nombre", user.Nombre},
            {"Email", user.Email},
            {"Direccion", user.Direccion}
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


        private UserRegistrarseDto ToDto(Usuario users)
        {
            return new UserRegistrarseDto()
            {
                UsuarioId = users.UsuarioId,
                Nombre = users.Nombre,
                Email = users.Email,
                Password = users.Password,
                Direccion = users.Direccion,
            };
        }
    }
}
