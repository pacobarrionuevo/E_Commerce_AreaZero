using static System.Net.Mime.MediaTypeNames;
using E_Commerce_VS.Models.Dto;
using E_Commerce_VS.Models.Database.Entidades;
using E_Commerce_VS.Extensions;

namespace E_Commerce_VS.Models.Mapper
{
    public class ProductoMapper
    {
        public ProductoDto ToDto(Producto producto, HttpRequest httpRequest = null)
        {
            return new ProductoDto()
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Ruta = httpRequest is null ? producto.Ruta : httpRequest.GetAbsoluteUrl(producto.Ruta),
            };
        }

        public IEnumerable<ProductoDto> ToDto(IEnumerable<Producto> productos, HttpRequest httpRequest = null)
        {
            return productos.Select(producto => ToDto(producto, httpRequest));
        }
    }
}
