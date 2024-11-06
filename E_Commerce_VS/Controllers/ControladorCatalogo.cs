using E_Commerce_VS.Models.Database.Entidades;
using E_Commerce_VS.Models.Dto;
using E_Commerce_VS.Models.Mapper;
using System.Linq;
using E_Commerce_VS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Climate;
using static E_Commerce_VS.Models.Database.Filtros.Enumerables;


namespace E_Commerce_VS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorCatalogo : ControllerBase
    {
        private readonly Services.ProductService _service;
        private readonly ProductoMapper _mapper;

        public ControladorCatalogo(Services.ProductService service, ProductoMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductoDto>> GetAllAsync(FiltroPrecio filtroPrecio =  FiltroPrecio.Ascendente, FiltroNombre filtroNombre = FiltroNombre.DeAaZ)
        {
            IEnumerable<Producto> productos = await _service.GetAllAsync();

            if (filtroNombre == FiltroNombre.DeAaZ && filtroPrecio == FiltroPrecio.Ascendente)
            {
                productos = productos.OrderBy(p => p.Nombre).ThenBy(p => p.Precio); 
            } 
            else if (filtroNombre == FiltroNombre.DeAaZ && filtroPrecio == FiltroPrecio.Descendente)
            {
                productos = productos.OrderBy(p => p.Nombre).ThenByDescending(p => p.Precio); 
            } 
            else if (filtroNombre == FiltroNombre.DeZaA && filtroPrecio == FiltroPrecio.Ascendente) 
            {
                productos = productos.OrderByDescending(p => p.Nombre).ThenBy(p => p.Precio); 
            } 
            else if (filtroNombre == FiltroNombre.DeZaA && filtroPrecio == FiltroPrecio.Descendente) 
            { 
                productos = productos.OrderByDescending(p => p.Nombre).ThenByDescending(p => p.Precio); 
            }

            return _mapper.ToDto(productos, Request);

        }


    }
}
