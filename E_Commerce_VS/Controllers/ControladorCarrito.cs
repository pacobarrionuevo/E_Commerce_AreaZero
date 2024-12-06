﻿using E_Commerce_VS.Models.Database.Entidades;
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

            // Buscar el producto utilizando el repositorio
            var producto = await _unitOfWork.RepoProd.GetByIdAsync(request.ProductId);
            if (producto == null)
            {
                return BadRequest("El producto no existe.");
            }

            // Buscar el carrito del usuario
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
        [Authorize]
        [HttpPost("PasaProductoAlCarrito")]
        public async Task<IActionResult> PasaProductoAlCarrito([FromBody] List<ProductoCarritoLocal> productos)
        {
            if (productos == null || !productos.Any())
            {
                return BadRequest("La lista de productos está vacía o es inválida.");
            }

            // Obtener el userId del token (de los claims)
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Usuario no válido.");
            }

            // Verificar si el carrito del usuario ya existe
            var carrito = await _unitOfWork.RepoCar.GetCarritoByUserIdAsync(userId);
            if (carrito == null)
            {
                carrito = new Carrito
                {
                    UserId = userId,
                    ProductoCarrito = new List<ProductoCarrito>()
                };

                // Agregar el nuevo carrito al contexto
                _unitOfWork.Context.Carritos.Add(carrito);
            }

            // Iterar por los productos recibidos
            foreach (var prod in productos)
            {
                if (prod.ProductId <= 0 || prod.Cantidad <= 0)
                {
                    return BadRequest($"El producto con ID {prod.ProductId} tiene datos inválidos.");
                }

                // Verificar si el producto existe en la base de datos
                var producto = await _unitOfWork.RepoProd.GetByIdAsync(prod.ProductId);
                if (producto == null)
                {
                    return NotFound($"El producto con ID {prod.ProductId} no existe.");
                }

                // Verificar si el producto ya está en el carrito
                var productoEnCarrito = carrito.ProductoCarrito.FirstOrDefault(p => p.ProductoId == prod.ProductId);
                if (productoEnCarrito != null)
                {
                    // Si ya está, aumentar la cantidad
                    productoEnCarrito.Cantidad += prod.Cantidad;
                }
                else
                {
                    // Si no está, añadirlo
                    carrito.ProductoCarrito.Add(new ProductoCarrito
                    {
                        ProductoId = prod.ProductId,
                        Cantidad = prod.Cantidad
                    });
                }
            }

            // Guardar los cambios en la base de datos
            await _unitOfWork.SaveAsync();

            // Retornar el carrito actualizado
            return Ok(carrito.ProductoCarrito);
        }

        [HttpPost("CreaOrdenTemporal")]
        public async Task<IActionResult> CrearOrdenTemporal([FromBody] List<ProductoCarritoLocal> productosLocales)
        {
            // Verificar si el usuario está logueado
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "id");
            int? userId = null;

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedUserId))
            {
                userId = parsedUserId;
            }

            // Crear lista de productos para la orden temporal
            List<ProductoCarrito> productosOrden = new();

            if (userId.HasValue)
            {
                // Usuario logueado: obtener productos del carrito en la base de datos
                var carrito = await _unitOfWork.RepoCar.GetCarritoByUserIdAsync(userId.Value);
                if (carrito == null || !carrito.ProductoCarrito.Any())
                {
                    return BadRequest("El carrito está vacío.");
                }

                productosOrden = carrito.ProductoCarrito.ToList();
            }
            else
            {
                // Usuario no logueado: usar productos locales
                if (productosLocales == null || !productosLocales.Any())
                {
                    return BadRequest("La lista de productos está vacía.");
                }

                foreach (var prod in productosLocales)
                {
                    if (prod.ProductId <= 0 || prod.Cantidad <= 0)
                    {
                        return BadRequest($"El producto con ID {prod.ProductId} tiene datos inválidos.");
                    }

                    productosOrden.Add(new ProductoCarrito
                    {
                        ProductoId = prod.ProductId,
                        Cantidad = prod.Cantidad
                    });
                }
            }

            // Verificar stock y calcular el precio total
            List<string> erroresStock = new();
            int precioTotal = 0;

            foreach (var item in productosOrden)
            {
                var producto = await _unitOfWork.RepoProd.GetByIdAsync(item.ProductoId);
                if (producto == null)
                {
                    erroresStock.Add($"Producto con ID {item.ProductoId} no existe.");
                    continue;
                }

                if (producto.Stock < item.Cantidad)
                {
                    erroresStock.Add($"Producto con ID {item.ProductoId} no tiene suficiente stock.");
                    continue;
                }

                // Reservar stock
                producto.Stock -= item.Cantidad;
                _unitOfWork.RepoProd.Update(producto);

                // Calcular precio total
                precioTotal += producto.Precio * item.Cantidad;
            }

            if (erroresStock.Any())
            {
                return BadRequest(new { Mensaje = "Error en el stock.", Detalles = erroresStock });
            }

            // Crear la orden temporal
            var ordenTemporal = new OrdenTemporal
            {
                FechaCreacion = DateTime.UtcNow,
                FechaExpiracion = DateTime.UtcNow.AddMinutes(3), // Expiración configurable
                UsuarioId = userId,
                Productos = productosOrden
            };

            await _unitOfWork.RepoOT.InsertAsync(ordenTemporal);
            await _unitOfWork.SaveAsync();

            // Devolver el ID de la orden temporal y el precio total
            return Ok(new
            {
                Mensaje = "Orden temporal creada con éxito.",
                OrdenId = ordenTemporal.Id,
                PrecioTotal = precioTotal
            });
        }



    }
<<<<<<< HEAD
    }
=======
}
>>>>>>> origin/salperro2
