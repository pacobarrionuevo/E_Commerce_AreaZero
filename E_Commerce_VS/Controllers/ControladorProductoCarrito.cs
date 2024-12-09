using E_Commerce_VS.Models.Database.Entidades;
using E_Commerce_VS.Models.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Commerce_VS.Models.Dto;

namespace E_Commerce_VS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorProductoCarrito : ControllerBase
    {
        private readonly ProyectoDbContext _context;

        public ControladorProductoCarrito(ProyectoDbContext dbContext)
        {
            _context = dbContext;
        }

        // Obtener todos los productos del carrito
        [HttpGet("productosCarrito")]
        public async Task<IEnumerable<ProductoCarrito>> GetProductosCarrito()
        {
            return await _context.ProductoCarritos.Include(pc => pc.Producto).ToListAsync();
        }

        [HttpPut("eliminarproductocarrito")]
        public async Task<IActionResult> DeleteProduct([FromBody] DeleteProductDto request)
        {
            // Buscar el carrito asociado al usuario
            var carrito = await _context.Carritos
                .FirstOrDefaultAsync(c => c.UserId == request.UserId);

            // Buscar el producto en el carrito
            var productoCarrito = await _context.ProductoCarritos
                .FirstOrDefaultAsync(pc => pc.ProductoId == request.ProductId && pc.CarritoId == carrito.Id);

            // Eliminarlo
            _context.ProductoCarritos.Remove(productoCarrito);
            await _context.SaveChangesAsync();

            return Ok("Producto eliminado del carrito.");
        }




        [HttpPut("cambiarcantidad")]
        public async Task<IActionResult> ModifyProduct([FromBody] ModifyProductDto request)
        {
            var productoCarrito = await _context.ProductoCarritos
                .FirstOrDefaultAsync(pc => pc.ProductoId == request.ProductId && pc.CarritoId == request.CarritoId);

            if (productoCarrito == null)
            {
                return NotFound("Producto no encontrado en el carrito.");
            }

            productoCarrito.Cantidad = request.Quantity;
            _context.ProductoCarritos.Update(productoCarrito);
            await _context.SaveChangesAsync();
            return Ok("Cantidad actualizada en el carrito.");
        }

        // Obtener productos de un carrito específico
        [HttpGet("productosCarrito/{userId}")]
        public async Task<IActionResult> GetProductosCarritoByUserId(int userId)
        {
            // Verificar si el carrito existe para el usuario
            var carrito = await _context.Carritos
                .Include(c => c.ProductoCarrito)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (carrito == null)
            {
                return NotFound($"No se encontró un carrito para el usuario con ID {userId}.");
            }

            // Obtener los productos asociados al carrito
            var productos = await _context.ProductoCarritos
                .Where(pc => pc.CarritoId == carrito.Id)
                .Include(pc => pc.Producto)
                .Select(pc => new
                {
                    pc.Id,
                    pc.ProductoId,
                    pc.CarritoId,
                    pc.Cantidad,
                    Producto = new
                    {
                        pc.Producto.Id,
                        pc.Producto.Nombre,
                        pc.Producto.Ruta,
                        pc.Producto.Precio,
                        pc.Producto.Stock
                    }
                })
                .ToListAsync();

            return Ok(productos);
        }
    }
}