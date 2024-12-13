﻿using E_Commerce_VS.Extensions;
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
            public async Task<IActionResult> GetOrdenesTemporales()
            {
                // Obtener todas las órdenes temporales activas
                var ordenesTemporales = await _dbContext.Set<OrdenTemporal>()
                    .Select(o => new
                    {
                        o.Id,
                        o.UsuarioId,
                        Productos = o.Productos.Select(pc => new
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

                return Ok(ordenesTemporales);
            }

        //Obtiene los detalles de una orden en concreto
        [HttpGet("order-details/{id}")]
        public async Task<IActionResult> GetOrderDetails(int id)
        {
            var orden = await _dbContext.OrdenesTemporales
                .Include(o => o.Productos)
                .ThenInclude(p => p.Producto)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden == null || orden.FechaExpiracion <= DateTime.UtcNow)
            {
                return NotFound("Orden no encontrada o expirada.");
            }

            var productos = orden.Productos.Select(p => new
            {
                p.Producto.Id,
                p.Producto.Nombre,
                p.Producto.Precio,
                p.Cantidad,
                Total = p.Cantidad * p.Producto.Precio
            });

            return Ok(new
            {
                Productos = productos,
                Total = productos.Sum(p => p.Total)
            });
        }

        //Muestra la tarjetita de Stripe creada por defecto
        [HttpPost("embedded")]
        public async Task<ActionResult> EmbeddedCheckout()
        {
            //Hacemos que lea el id del token y que reciba la orden temporal por ese id
            int userIdToken = Int32.Parse(User.FindFirst("id").Value);

            var ordenTemporal = _dbContext.OrdenesTemporales.Include(o => o.Productos).FirstOrDefault(o => o.UsuarioId == userIdToken);

            var lineItems = new List<SessionLineItemOptions>();

            //Se deberian ir guardando todos los productos que hay en la orden temporal
            foreach (var producto in ordenTemporal.Productos)
            {
                var productoStripe = _dbContext.Productos.FirstOrDefault(b => b.Id == producto.Id);

                if (productoStripe != null)
                {
                    lineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "eur",
                            UnitAmount = (long)(productoStripe.Precio),
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = productoStripe.Nombre,
                                Description = productoStripe.Descripcion,
                                Images = new List<string> { productoStripe.Ruta }
                            }
                        },
                        Quantity = producto.Cantidad
                    });
                }
            }

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

            return Ok(new { clientSecret = session.ClientSecret, sessionUrl = session.Url });
        }

        //Devuelve el estado de la session
        [HttpGet("status/{sessionId}")]
        public async Task<ActionResult> SessionStatus(string sessionId)
        {
            var sessionService = new SessionService();
            var session = await sessionService.GetAsync(sessionId);

            return Ok(new { status = session.Status, customerEmail = session.CustomerEmail });
        }

        //CREA UNA ORDEN TEMPORAL GIGANTE
        [HttpPost("CrearOrdenTemporal")]
        public async Task<IActionResult> CrearOrdenTemporal([FromBody] List<ProductoCheckoutDto>? productosCarrito, int? userId, int? ordenId)
        {
            //Se hace cuando se da al boton de pagar en el front
            if ((productosCarrito == null || !productosCarrito.Any()) && userId.HasValue)
            {
                var carritoUsuario = await _dbContext.Carritos
                    .Include(c => c.ProductoCarrito)
                    .ThenInclude(cp => cp.Producto)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (carritoUsuario == null || !carritoUsuario.ProductoCarrito.Any())
                {
                    return BadRequest("No se encontraron productos en el carrito del usuario.");
                }

                productosCarrito = carritoUsuario.ProductoCarrito.Select(cp => new ProductoCheckoutDto
                {
                    ProductoId = cp.ProductoId,
                    Cantidad = cp.Cantidad,
                }).ToList();
            }

            if (productosCarrito == null || !productosCarrito.Any())
            {
                return BadRequest("No se proporcionaron productos válidos para crear la orden temporal.");
            }

            if (ordenId.HasValue)
            {
                // Busca la orden activa para ver cual es la necesaria
                var ordenActiva = await _dbContext.OrdenesTemporales
                    .Include(o => o.Productos)
                    .FirstOrDefaultAsync(o => o.Id == ordenId && o.FechaExpiracion > DateTime.UtcNow);

                if (ordenActiva != null)
                {
                    if (userId.HasValue)
                    {
                        ordenActiva.UsuarioId = userId.Value;
                    }

                    foreach (var productoCarrito in productosCarrito)
                    {
                        var producto = await _dbContext.Productos.FindAsync(productoCarrito.ProductoId);
                        if (producto == null)
                        {
                            return NotFound($"Producto con ID {productoCarrito.ProductoId} no encontrado.");
                        }

                        if (producto.Stock < productoCarrito.Cantidad)
                        {
                            return BadRequest($"Stock insuficiente para el producto {producto.Nombre}. Disponible: {producto.Stock}, solicitado: {productoCarrito.Cantidad}.");
                        }

                        var productoEnOrden = ordenActiva.Productos
                            .FirstOrDefault(p => p.ProductoId == productoCarrito.ProductoId);

                        //Reserva de stock
                        if (productoEnOrden != null)
                        {
                            producto.Stock += productoEnOrden.Cantidad;
                            productoEnOrden.Cantidad = productoCarrito.Cantidad;
                        }
                        else
                        {
                            ordenActiva.Productos.Add(new ProductoCarrito
                            {
                                ProductoId = productoCarrito.ProductoId,
                                Cantidad = productoCarrito.Cantidad,
                            });
                        }

                        // Actualizar stock con la nueva cantidad dicha
                        producto.Stock -= productoCarrito.Cantidad; 
                    }

                    ordenActiva.Productos.RemoveAll(p =>
                        !productosCarrito.Any(pc => pc.ProductoId == p.ProductoId));

                    ordenActiva.FechaExpiracion = DateTime.UtcNow.AddMinutes(30);

                    await _dbContext.SaveChangesAsync();

                    return Ok(new
                    {
                        Mensaje = "Orden temporal actualizada.",
                        OrdenId = ordenActiva.Id
                    });
                }

                return BadRequest("La orden temporal no está activa o ha expirado.");
            }

            //Aqui si la orden temporal es nueva
            var nuevaOrden = new OrdenTemporal
            {
                UsuarioId = userId,
                Productos = new List<ProductoCarrito>(),
                FechaExpiracion = DateTime.UtcNow.AddMinutes(30)
            };

            foreach (var productoCarrito in productosCarrito)
            {
                var producto = await _dbContext.Productos.FindAsync(productoCarrito.ProductoId);
                if (producto == null)
                {
                    return NotFound($"Producto con ID {productoCarrito.ProductoId} no encontrado.");
                }

                if (producto.Stock < productoCarrito.Cantidad)
                {
                    return BadRequest($"Stock insuficiente para el producto {producto.Nombre}. Disponible: {producto.Stock}, solicitado: {productoCarrito.Cantidad}.");
                }

                producto.Stock -= productoCarrito.Cantidad;

                nuevaOrden.Productos.Add(new ProductoCarrito
                {
                    ProductoId = productoCarrito.ProductoId,
                    Cantidad = productoCarrito.Cantidad,
                });
            }

            _dbContext.OrdenesTemporales.Add(nuevaOrden);
            await _dbContext.SaveChangesAsync();

            return Ok(new
            {
                Mensaje = "Nueva orden temporal creada.",
                OrdenId = nuevaOrden.Id
            });
        }

    }
}
