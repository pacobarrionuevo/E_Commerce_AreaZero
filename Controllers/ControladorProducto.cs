using E_Commerce_VS.Models.Database.Entidades;
using E_Commerce_VS.Models.Dto;
using E_Commerce_VS.Models.Mapper;
using E_Commerce_VS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace E_Commerce_VS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorProducto : ControllerBase
    {
        private readonly ProductService _service;
        private readonly ProductoMapper _mapper;

        public ControladorProducto(Services.ProductService servicio, ProductoMapper mapper)
        {
            _service = servicio;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductoDto>> GetAllAsync()
        {
            IEnumerable<Producto> productos = await _service.GetAllAsync();

            return _mapper.ToDto(productos, Request); 
        }

        [HttpGet("{id}")]
        public async Task<ProductoDto> GetAsync(long id)
        {
            Producto prod = await _service.GetAsync(id);

            return _mapper.ToDto(prod, Request);
        }

        [HttpPost]
        public async Task<ActionResult<ProductoDto>> InsertAsync(CreateUpdateProductoRequest createProd)
        {
            Producto prod = await _service.InsertAsync(createProd);

            return Created($"images/{prod.Id}", _mapper.ToDto(prod));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductoDto>> UpdateAsync(long id, CreateUpdateProductoRequest updateProd)
        {
            Producto prod = await _service.UpdateAsync(id, updateProd);

            return Ok(_mapper.ToDto(prod));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductoDto>> DeleteAsync(long id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
