﻿using E_Commerce_VS.Models.Database.Entidades;
using E_Commerce_VS.Models.Dto;
using E_Commerce_VS.Models.Mapper;
using E_Commerce_VS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using static E_Commerce_VS.Models.Database.Filtros.Enumerables;
using E_Commerce_VS.Models.Database.Paginación;

namespace E_Commerce_VS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorCatalogo : ControllerBase
    {
        private readonly ProductService _service;
        private readonly ProductoMapper _mapper;
        private readonly SmartSearchService _smartSearchService;

        public ControladorCatalogo(ProductService service, ProductoMapper mapper, SmartSearchService smartSearchService)
        {
            _service = service;
            _mapper = mapper;
            _smartSearchService = smartSearchService;
        }

        [HttpGet]
        public async Task<Paginacion<ProductoDto>> GetAllAsync(
            Ordenacion filtro = Ordenacion.AscendenteNombre,
            int paginaActual = 1,
            int elementosPorPagina = 10,

            string query = "")  // Parámetro opcional para la búsqueda

        {
            // Obtiene todos los productos desde el servicio
            IEnumerable<Producto> productos = await _service.GetAllAsync();

            // Si hay un término de búsqueda, aplica SmartSearchService para obtener coincidencias
            if (!string.IsNullOrEmpty(query))
            {
                var searchResults = _smartSearchService.Search(query);
                productos = productos.Where(p => searchResults.Contains(p.Nombre));
            }

            // Aplica el filtro de ordenación seleccionado
            productos = filtro switch
            {
                Ordenacion.AscendenteNombre => productos.OrderBy(p => p.Nombre),
                Ordenacion.DescendenteNombre => productos.OrderByDescending(p => p.Nombre),
                Ordenacion.AscendentePrecio => productos.OrderBy(p => p.Precio),
                Ordenacion.DescendentePrecio => productos.OrderByDescending(p => p.Precio),
                _ => productos
            };

            // Paginación
            var totalElementos = productos.Count();
            var totalPaginas = (int)Math.Ceiling(totalElementos / (double)elementosPorPagina);
            var productosPaginados = productos
                .Skip((paginaActual - 1) * elementosPorPagina)
                .Take(elementosPorPagina);

            // Mapea los productos paginados a DTO
            var productosDto = _mapper.ToDto(productosPaginados, Request);

            // Retorna los resultados paginados y filtrados
            return new Paginacion<ProductoDto>
            {
                Resultados = productosDto,
                NumeroFilas = totalElementos,
                TotalPaginas = totalPaginas,
                ElementosPorPagina = elementosPorPagina,
                PaginaActual = paginaActual
            };
        }
        // Método para obtener un producto por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            var producto = await _service.GetAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            var productoDto = _mapper.ToDto(producto, Request);
            return Ok(productoDto);
        }
    }
}
