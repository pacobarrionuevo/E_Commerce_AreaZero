using E_Commerce_VS.Models.Database.Entidades;
using E_Commerce_VS.Models.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Commerce_VS.Models.Dto;
using Microsoft.AspNetCore.Authorization;

namespace E_Commerce_VS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorCarrito : ControllerBase
    {
        private readonly ProyectoDbContext _context;
        private readonly UnitOfWork _unitOfWork;

        public ControladorCarrito(ProyectoDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet("carritos")]
        public async Task<IActionResult> GetCarritos()
        {
            var carritos = await _context.Carritos
                .Select(c => new
                {
                    c.Id,
                    c.UserId,
                    Productos = c.ProductoCarrito.Select(pc => new
                    {
                        pc.ProductoId,
                        pc.Cantidad
                    })
                })
                .ToListAsync();

            return Ok(carritos);
        }

        // Añadir un producto al carrito
        [HttpPost("addtoshopcart")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductDto request)
        {
            if (request.Quantity <= 0)
            {
                return BadRequest("La cantidad debe ser mayor a 0.");
            }

            var producto = await _context.Productos.FindAsync(request.ProductId);
            if (producto == null)
            {
                return BadRequest("El producto no existe.");
            }

            var carrito = await _context.Carritos
                .Include(c => c.ProductoCarrito)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId);

            if (carrito == null)
            {
                carrito = new Carrito
                {
                    UserId = request.UserId,
                    ProductoCarrito = new List<ProductoCarrito>()
                };

                await _context.Carritos.AddAsync(carrito);
                await _context.SaveChangesAsync();
            }

            var productoCarrito = carrito.ProductoCarrito.FirstOrDefault(pc => pc.ProductoId == request.ProductId);
            if (productoCarrito != null)
            {
                productoCarrito.Cantidad += request.Quantity;
            }
            else
            {
                productoCarrito = new ProductoCarrito
                {
                    ProductoId = request.ProductId,
                    CarritoId = carrito.Id,
                    Cantidad = request.Quantity
                };
                carrito.ProductoCarrito.Add(productoCarrito);
            }

            await _context.SaveChangesAsync();
            return Ok("Producto añadido o actualizado en el carrito.");
        }

        // Asociar un carrito anónimo a un usuario registrado
        [HttpPost("associate-cart")]
        public async Task<IActionResult> AssociateCart(int userId)
        {
            // Buscar el carrito anónimo (sin UserId)
            var carritoAnonimo = await _context.Carritos
                .Include(c => c.ProductoCarrito)
                .FirstOrDefaultAsync(c => c.UserId == null);

            if (carritoAnonimo != null)
            {
                // Buscar el carrito del usuario registrado, si existe
                var carritoUsuario = await _context.Carritos
                    .Include(c => c.ProductoCarrito)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (carritoUsuario == null)
                {
                    // Si el usuario no tiene carrito, asignar el carrito anónimo
                    carritoAnonimo.UserId = userId;
                }
                else
                {
                    // Si ya tiene carrito, combinar productos
                    foreach (var productoAnonimo in carritoAnonimo.ProductoCarrito)
                    {
                        var productoEnCarrito = carritoUsuario.ProductoCarrito
                            .FirstOrDefault(pc => pc.ProductoId == productoAnonimo.ProductoId);

                        if (productoEnCarrito != null)
                        {
                            // Sumar cantidades si el producto ya está en el carrito del usuario
                            productoEnCarrito.Cantidad += productoAnonimo.Cantidad;
                        }
                        else
                        {
                            // Añadir el producto al carrito del usuario
                            carritoUsuario.ProductoCarrito.Add(new ProductoCarrito
                            {
                                ProductoId = productoAnonimo.ProductoId,
                                Cantidad = productoAnonimo.Cantidad
                            });
                        }
                    }

                    // Eliminar el carrito anónimo después de combinar
                    _context.Carritos.Remove(carritoAnonimo);
                }

                await _context.SaveChangesAsync();
                return Ok("Carrito asociado al usuario.");
            }

            return BadRequest("No hay un carrito anónimo para asociar.");
        }
        
        
        [Authorize]
        [HttpPost("PasaProductoAlCarrito")]
        public async Task<IActionResult<ICollection<ProductoCarritoLocal>>> PasaProductoAlCarrito([FromBody] ProductoCarritoLocal prod )
        {
            if (prod == null || prod.ProductId <= 0 || prod.Cantidad <= 0)
            {
                return BadRequest("Producto inválido. Asegúrate de que el ID del producto y la cantidad sean válidos.");
            }

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Usuario no válido.");
            }

            var producto = await _unitOfWork.RepoProd.GetByIdAsync(prod.ProductId);
            if (producto == null)
            {
                return NotFound("El producto especificado no existe.");
            }
        }
        
    }
}
