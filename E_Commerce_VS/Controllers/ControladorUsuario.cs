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

        //Controlador que devuelve una lista de todos los usuarios
        [HttpGet("userlist")]
        public IEnumerable<UserRegistrarseDto> GetUser()
        {
            return _context.Usuarios.Select(ToDto);
        }

        //Metodo para registrar usuario
        [HttpPost("Registro")]
        public async Task<IActionResult> Register([FromBody] UserRegistrarseDto usuario)
        {
            //Comprobacion de que no exista ya el usuario
            if (_context.Usuarios.Any(Usuario => Usuario.Nombre == usuario.Nombre))
            {
                return BadRequest("El nombre del usuario ya está en uso");
            }

            //Crea el usuario, lo almacena y lo mappea con el DTO
            Usuario newUser = new Usuario()
            {
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Password = PasswordHelper.Hash(usuario.Password),
                Direccion = usuario.Direccion,
                esAdmin = usuario.esAdmin
            };

            await _context.Usuarios.AddAsync(newUser);
            await _context.SaveChangesAsync();
            UserRegistrarseDto userCreated = ToDto(newUser);

            //Token, y le pasamos por claims todo lo que necesitamos para varias clases, como la vista admin (ej.)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = new Dictionary<string, object>
                {
                    {"id", newUser.UsuarioId},
                    {"Nombre", newUser.Nombre},
                    {"Email", newUser.Email},
                    {"Direccion", newUser.Direccion},
                    {"esAdmin", newUser.esAdmin}
                },
                Expires = DateTime.UtcNow.AddDays(5),
                SigningCredentials = new SigningCredentials(
                    _tokenParameters.IssuerSigningKey,
                    SecurityAlgorithms.HmacSha256Signature)
            };

            //Crea el token lo guarda lo hashea y da OK
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string accessToken = tokenHandler.WriteToken(token);
            return Ok(new { StringToken = accessToken });
        }

        //Metodo para que un cliente inicie sesion
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto userLoginDto)
        {

            //Comprueba que tanto el email como la password sean correctas
            var user = _context.Usuarios.FirstOrDefault(u => u.Email == userLoginDto.Email);
            if (user == null)
            {
                return Unauthorized("Usuario no existe");
            }

            if (!PasswordHelper.Hash(userLoginDto.Password).Equals(user.Password))
            {
                return Unauthorized("Contraseña incorrecta");
            }

            //Token con todo lo que necesita (igual que Registro)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = new Dictionary<string, object>
                {
                    {"id", user.UsuarioId},
                    {"Nombre", user.Nombre},
                    {"Email", user.Email},
                    {"Direccion", user.Direccion},
                    {"esAdmin", user.esAdmin}
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

        //Metodo para actualizar un usuario, se llama en el cliente para la vista admin
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
            user.esAdmin = userUpdateDto.esAdmin;

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
                    {"esAdmin", user.esAdmin}
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

        //Creamos aquí el Dto para tenerlo a mano
        private UserRegistrarseDto ToDto(Usuario users)
        {
            return new UserRegistrarseDto()
            {
                UsuarioId = users.UsuarioId,
                Nombre = users.Nombre,
                Email = users.Email,
                Password = users.Password,
                Direccion = users.Direccion,
                esAdmin = users.esAdmin
            };
        }
    }
}
