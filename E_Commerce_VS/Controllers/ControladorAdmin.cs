using E_Commerce_VS.Models.Database;
using E_Commerce_VS.Models.Database.Entidades;
using E_Commerce_VS.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_VS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorAdmin : ControllerBase
    {
        private readonly ProyectoDbContext _context;

        public ControladorAdmin(ProyectoDbContext context)
        {
            _context = context;
        }

        // Lista usuarios
        [HttpGet("usuarios")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsuarios()
        {
            return await _context.Usuarios
                .Select(u => new UserDto
                {
                    Id = u.UsuarioId,
                    Nombre = u.Nombre,
                    Email = u.Email,
                    esAdmin = u.esAdmin
                })
                .ToListAsync();
        }

        //  Para Modificar si es Admin un usuario
        [HttpPut("usuarios/{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] UserUpdateDto userUpdateDto)
        {
            var user = await _context.Usuarios.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.esAdmin = userUpdateDto.esAdmin;
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Borrar usuario siendo admin
        [HttpDelete("usuarios/{adminId}/{userId}")]
        public async Task<IActionResult> DeleteUsuario(int adminId, int userId)
        {
            var adminUser = await _context.Usuarios.FindAsync(adminId);
            if (adminUser == null || !adminUser.esAdmin)
            {
                return Unauthorized("No tienes permisos de administrador para realizar esta acción.");
            }

            var userToDelete = await _context.Usuarios.FindAsync(userId);
            if (userToDelete == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            _context.Usuarios.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Lista productos
        [HttpGet("productos")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductos()
        {
            return await _context.Productos
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Ruta = p.Ruta,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio
                    
                })
                .ToListAsync();
        }

        // Modificar producto siendo admin
        [HttpPut("productos/{id}")]
        public async Task<IActionResult> UpdateProducto(long id, [FromBody] ProductUpdateDto productUpdateDto)
        {
            if (productUpdateDto == null)
            {
                return BadRequest("El campo productUpdateDto es requerido.");
            }

            var product = await _context.Productos.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            product.Id = productUpdateDto.Id;
            product.Nombre = productUpdateDto.Nombre;
            product.Ruta = productUpdateDto.Ruta;
            product.Descripcion = productUpdateDto.Descripcion;
            product.Precio = productUpdateDto.Precio;
            product.Stock = productUpdateDto.Stock;
            // Añadir otros campos necesarios
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // Comprueba si el usuario existe
        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }

        //Comprueba si el producto existe
        private bool ProductoExists(long id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
