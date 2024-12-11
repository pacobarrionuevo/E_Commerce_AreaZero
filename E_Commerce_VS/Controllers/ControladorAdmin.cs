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

<<<<<<< HEAD
        // Para Modificar si es Admin un usuario
=======
        // Para modificar si es admin un usuario
>>>>>>> origin/main
        [HttpPut("usuarios/{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] UserUpdateDto userUpdateDto)
        {
            if (userUpdateDto == null)
            {
                return BadRequest("El cuerpo de la solicitud es nulo.");
            }

            var user = await _context.Usuarios.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Nombre = userUpdateDto.Nombre;
            user.Email = userUpdateDto.Email;
            user.Direccion = userUpdateDto.Direccion;
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
        [HttpDelete("usuarios/{userId}")]
        public async Task<IActionResult> DeleteUsuario(int userId)
        {
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

        // Crear nuevo producto
        [HttpPost("productos")]
        public async Task<ActionResult<ProductoDto>> CreateProducto([FromBody] ProductCreateDto productCreateDto)
        {
            if (productCreateDto == null)
            {
                return BadRequest("El cuerpo de la solicitud es nulo.");
            }

            var product = new Producto
            {
                Nombre = productCreateDto.Nombre,
                Ruta = productCreateDto.Ruta,
                Descripcion = productCreateDto.Descripcion,
                Precio = productCreateDto.Precio,
                Stock = productCreateDto.Stock
            };

            _context.Productos.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateProducto), new { id = product.Id }, product);
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

        // Borrar producto
        [HttpDelete("productos/{id}")]
        public async Task<IActionResult> DeleteProducto(long id)
        {
            var product = await _context.Productos.FindAsync(id);
            if (product == null)
            {
                return NotFound("Producto no encontrado.");
            }

            _context.Productos.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Comprueba si el usuario existe
        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }

        // Comprueba si el producto existe
        private bool ProductoExists(long id)
        {
            return _context.Productos.Any(e => e.Id == id);
        }
    }
}
