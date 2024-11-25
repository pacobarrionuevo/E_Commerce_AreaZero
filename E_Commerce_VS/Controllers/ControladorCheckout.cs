using E_Commerce_VS.Extensions;
using E_Commerce_VS.Models.Database;
using E_Commerce_VS.Models.Database.Entidades;
using E_Commerce_VS.Models.Dto;
using E_Commerce_VS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe.Checkout;

namespace E_Commerce_VS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorCheckout : ControllerBase
    {
        private readonly Settings _settings;
        private readonly ProductService _productService;
        private readonly ProyectoDbContext _dbContext;

        public ControladorCheckout(IOptions<Settings> options, ProductService productService, ProyectoDbContext contexto)
        {
            _settings = options.Value;
            _productService = productService;
            _dbContext = contexto;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetCarrito()
        {
            var carritos = await _dbContext.Set<Carrito>()
                .Select(c => new
                {
                    c.Id,
                    c.UserId,
                    Productos = c.ProductoCarrito.Select(pc => new
                    {
                        pc.ProductoId,
                        pc.Cantidad,
                        Producto = new ProductoDto
                        {
                            Id = pc.Producto.Id,
                            Nombre = pc.Producto.Nombre,
                            Ruta = pc.Producto.Ruta,
                            Precio = pc.Producto.Precio,
                            Stock = pc.Producto.Stock
                        }
                    })
                })
                .ToListAsync();

            return Ok(carritos);
        }

        [HttpPost("hosted")]
        public async Task<ActionResult> HostedCheckout([FromBody] List<ProductoDto> productos)
        {
            if (productos == null || !productos.Any())
                return BadRequest("No se proporcionaron productos para el checkout.");

            var lineItems = productos.Select(product => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "eur",
                    UnitAmount = (long)(product.Precio * 100),
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Nombre,
                        Images = new List<string> { product.Ruta }
                    }
                },
                Quantity = 1
            }).ToList();

            var options = new SessionCreateOptions
            {
                UiMode = "hosted",
                Mode = "payment",
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                CustomerEmail = "correo_cliente@correo.es",
                SuccessUrl = _settings.ClientBaseUrl + "/checkout?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = _settings.ClientBaseUrl + "/checkout"
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Ok(new { sessionUrl = session.Url });
        }

        [HttpPost("embedded")]
        public async Task<ActionResult> EmbeddedCheckout([FromBody] List<ProductoDto> productos)
        {
            if (productos == null || !productos.Any())
                return BadRequest("No se proporcionaron productos para el checkout.");

            var lineItems = productos.Select(product => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "eur",
                    UnitAmount = (long)(product.Precio * 100),
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Nombre,
                        Images = new List<string> { product.Ruta }
                    }
                },
                Quantity = 1
            }).ToList();

            var options = new SessionCreateOptions
            {
                UiMode = "embedded",
                Mode = "payment",
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                CustomerEmail = "correo_cliente@correo.es",
                ReturnUrl = _settings.ClientBaseUrl + "/checkout?session_id={CHECKOUT_SESSION_ID}"
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return Ok(new { clientSecret = session.ClientSecret });
        }

        [HttpGet("status/{sessionId}")]
        public async Task<ActionResult> SessionStatus(string sessionId)
        {
            var sessionService = new SessionService();
            var session = await sessionService.GetAsync(sessionId);

            return Ok(new { status = session.Status, customerEmail = session.CustomerEmail });
        }

        [HttpPost("CrearOrdenTemporal")]
        public async Task<ActionResult> CrearOrdenTemporal([FromBody] List<ProductoCarritoDto> productosCarrito)
        {
            if (productosCarrito == null || !productosCarrito.Any())
                return BadRequest("El carrito está vacío.");

            var ordenTemporal = new OrdenTemporal
            {
                FechaCreacion = DateTime.UtcNow,
                FechaExpiracion = DateTime.UtcNow.AddMinutes(5), 
                Productos = new List<ProductoOrdenTemporal>()
            };

            foreach (var productoCarrito in productosCarrito)
            {
                var producto = await _dbContext.Productos.FindAsync(productoCarrito.ProductoId);

                if (producto == null)
                    return NotFound($"Producto con ID {productoCarrito.ProductoId} no encontrado.");

                if (producto.Stock < productoCarrito.Cantidad)
                {
                    return BadRequest(new
                    {
                        Mensaje = $"No hay suficiente stock para el producto: {producto.Nombre}. Disponible: {producto.Stock}, Solicitado: {productoCarrito.Cantidad}"
                    });
                }

                producto.Stock -= productoCarrito.Cantidad;

                ordenTemporal.Productos.Add(new ProductoOrdenTemporal
                {
                    ProductoId = (int)producto.Id,
                    Cantidad = productoCarrito.Cantidad,
                    PrecioUnitario = producto.Precio
                });
            }

            _dbContext.OrdenesTemporales.Add(ordenTemporal);
            await _dbContext.SaveChangesAsync();

            return Ok(new
            {
                OrdenId = ordenTemporal.Id,
                Mensaje = "Orden temporal creada exitosamente.",
                ProductosReservados = ordenTemporal.Productos.Select(p => new
                {
                    p.ProductoId,
                    p.Cantidad
                })
            });
        }
    }
}
