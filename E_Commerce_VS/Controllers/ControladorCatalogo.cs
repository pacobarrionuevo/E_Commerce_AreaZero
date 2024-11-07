using E_Commerce_VS.Models.Database.Entidades;
using E_Commerce_VS.Models.Dto;
using E_Commerce_VS.Models.Mapper;
using E_Commerce_VS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public ControladorCatalogo(ProductService service, ProductoMapper mapper)
        {
            _service = service;
            _mapper = mapper;
            _smartSearchService = new SmartSearchService();
        }

        // Endpoint de paginación y búsqueda combinados
        [HttpGet]
        public async Task<Paginacion<ProductoDto>> GetAllAsync(
            FiltroPrecio filtroPrecio = FiltroPrecio.Ascendente,
            FiltroNombre filtroNombre = FiltroNombre.DeAaZ,
            int paginaActual = 1,
            int elementosPorPagina = 10,
            string query = "")  // Parámetro opcional para la búsqueda
        {
            // Obtención de productos desde el servicio
            IEnumerable<Producto> productos = await _service.GetAllAsync();

            // Si hay un término de búsqueda, filtramos los productos por nombre
            if (!string.IsNullOrEmpty(query))
            {
                productos = productos.Where(p => p.Nombre.Contains(query, StringComparison.OrdinalIgnoreCase));
            }

            // Primero, ordenamos por el filtro de nombre
            IOrderedEnumerable<Producto> productosOrdenados;

            switch (filtroNombre)
            {
                case FiltroNombre.DeAaZ:
                    productosOrdenados = productos.OrderBy(p => p.Nombre); // Ascendente por nombre
                    break;

                case FiltroNombre.DeZaA:
                    productosOrdenados = productos.OrderByDescending(p => p.Nombre); // Descendente por nombre
                    break;

                default:
                    productosOrdenados = productos.OrderBy(p => p.Nombre); // Default: Ascendente por nombre
                    break;
            }

            // Luego, aplicamos el filtro de precio
            switch (filtroPrecio)
            {
                case FiltroPrecio.Ascendente:
                    productosOrdenados = productosOrdenados.ThenBy(p => p.Precio); // Ascendente por precio
                    break;

                case FiltroPrecio.Descendente:
                    productosOrdenados = productosOrdenados.ThenByDescending(p => p.Precio); // Descendente por precio
                    break;

                default:
                    productosOrdenados = productosOrdenados.ThenBy(p => p.Precio); // Default: Ascendente por precio
                    break;
            }



            // Paginación
            var totalElementos = productos.Count();
            var totalPaginas = (int)Math.Ceiling(totalElementos / (double)elementosPorPagina);
            var productosPaginados = productos
                .Skip((paginaActual - 1) * elementosPorPagina)
                .Take(elementosPorPagina);

            // Mapeo a DTO
            var productosDto = _mapper.ToDto(productosPaginados, Request);

            // Retorno del resultado paginado y filtrado
            return new Paginacion<ProductoDto>
            {
                Resultados = productosDto,
                NumeroFilas = totalElementos,
                TotalPaginas = totalPaginas,
                ElementosPorPagina = elementosPorPagina,
                PaginaActual = paginaActual
            };
        }
    }
}
