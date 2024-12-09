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

        // Eliminar un producto del carrito
        [HttpPut("eliminarproductocarrito")]
        public async Task<IActionResult> DeleteProduct([FromBody] DeleteProductDto request)
        {
            var productoCarrito = await _context.ProductoCarritos
                .FirstOrDefaultAsync(pc => pc.ProductoId == request.ProductId && pc.CarritoId == request.CarritoId);

            if (productoCarrito == null)
            {
                return NotFound("Producto no encontrado en el carrito.");
            }

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
        [HttpGet("productosCarrito/{carritoId}")]
        public async Task<IActionResult> GetProductosCarrito(int carritoId)
        {
            var productos = await _context.ProductoCarritos
                .Where(pc => pc.CarritoId == carritoId)
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