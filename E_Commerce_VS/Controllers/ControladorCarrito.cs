using E_Commerce_VS.Models.Database.Entidades;
using E_Commerce_VS.Models.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_Commerce_VS.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace E_Commerce_VS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorCarrito : ControllerBase
    {
        private readonly ProyectoDbContext _context;
        private readonly UnitOfWork _unitOfWork;

        public ControladorCarrito(ProyectoDbContext dbContext, UnitOfWork unitOfWork)
        {
            _context = dbContext;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        //Devuelve todos los carritos 
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

            var producto = await _unitOfWork.RepoProd.GetByIdAsync(request.ProductId);
            if (producto == null)
            {
                return BadRequest("El producto no existe.");
            }

            var carrito = await _unitOfWork.RepoCar.GetCarritoByUserIdAsync(request.UserId);

            if (carrito == null)
            {
                carrito = new Carrito
                {
                    UserId = request.UserId,
                    ProductoCarrito = new List<ProductoCarrito>()
                };

                await _unitOfWork.RepoCar.InsertAsync(carrito);
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

            await _unitOfWork.SaveAsync();
            return Ok("Producto añadido o actualizado en el carrito.");
        }


        // Asociar un carrito anonimo a un usuario registrado
        [HttpPost("associate-cart")]
        public async Task<IActionResult> AssociateCart(int userId)
        {
            var carritoAnonimo = await _context.Carritos
                .Include(c => c.ProductoCarrito)
                .FirstOrDefaultAsync(c => c.UserId == null);

            if (carritoAnonimo != null)
            {
                var carritoUsuario = await _context.Carritos
                    .Include(c => c.ProductoCarrito)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (carritoUsuario == null)
                {
                    carritoAnonimo.UserId = userId;
                }
                else
                {
                    foreach (var productoAnonimo in carritoAnonimo.ProductoCarrito)
                    {
                        var productoEnCarrito = carritoUsuario.ProductoCarrito
                            .FirstOrDefault(pc => pc.ProductoId == productoAnonimo.ProductoId);

                        if (productoEnCarrito != null)
                        {

                            productoEnCarrito.Cantidad += productoAnonimo.Cantidad;
                        }
                        else
                        {
                            carritoUsuario.ProductoCarrito.Add(new ProductoCarrito
                            {
                                ProductoId = productoAnonimo.ProductoId,
                                Cantidad = productoAnonimo.Cantidad
                            });
                        }
                    }

                    _context.Carritos.Remove(carritoAnonimo);
                }

                await _context.SaveChangesAsync();
                return Ok("Carrito asociado al usuario.");
            }

            return BadRequest("No hay un carrito anónimo para asociar.");
        }


        //Pasa un producto de la orden temporal al carrito
        [Authorize]
        [HttpPost("PasaProductoAlCarrito")]
        public async Task<IActionResult> PasaProductoAlCarrito([FromBody] List<ProductoCarritoLocal> productos)
        {
            //otro que novea el trabajito que nos costo
            if (productos == null || !productos.Any())
            {
                return BadRequest("La lista de productos está vacía o es inválida.");
            }

            // Obtener el userId del token desde los claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Usuario no válido.");
            }

            var carrito = await _unitOfWork.RepoCar.GetCarritoByUserIdAsync(userId);
            if (carrito == null)
            {
                carrito = new Carrito
                {
                    UserId = userId,
                    ProductoCarrito = new List<ProductoCarrito>()
                };

                _unitOfWork.Context.Carritos.Add(carrito);
            }

            // Todos los productos recibidos se pasan por este bucle
            foreach (var prod in productos)
            {
                if (prod.ProductId <= 0 || prod.Cantidad <= 0)
                {
                    return BadRequest($"El producto con ID {prod.ProductId} tiene datos inválidos.");
                }

                // Verificar si el producto existe en la base de datos y luego mirar si esta en el carrito
                var producto = await _unitOfWork.RepoProd.GetByIdAsync(prod.ProductId);
                if (producto == null)
                {
                    return NotFound($"El producto con ID {prod.ProductId} no existe.");
                }

                var productoEnCarrito = carrito.ProductoCarrito.FirstOrDefault(p => p.ProductoId == prod.ProductId);
                if (productoEnCarrito != null)
                {
 
                    productoEnCarrito.Cantidad += prod.Cantidad;
                }
                else
                {
                    // Si el producto no esta, añadirlo
                    carrito.ProductoCarrito.Add(new ProductoCarrito
                    {
                        ProductoId = prod.ProductId,
                        Cantidad = prod.Cantidad
                    });
                }
            }

            await _unitOfWork.SaveAsync();
            return Ok(carrito.ProductoCarrito);
        }
    }

}