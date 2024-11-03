using E_Commerce_VS.Models.Database;
using E_Commerce_VS.Models.Database.Entidades;
using E_Commerce_VS.Models.Dto;
using E_Commerce_VS.Recursos;

namespace E_Commerce_VS.Services
{
    public class ProductService
    {
        private const string PRODUCTS_FOLDER = "images";

        private readonly UnitOfWork _unitOfWork;

        public ProductService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<ICollection<Producto>> GetAllAsync()
        {
            return _unitOfWork.RepoProd.GetAllAsync();
        }

        public Task<Producto> GetAsync(long id)
        {
            return _unitOfWork.RepoProd.GetByIdAsync(id);
        }

        public async Task<Producto> InsertAsync(CreateUpdateProductoRequest prodReq)
        {
            string relativePath = $"{PRODUCTS_FOLDER}/{Guid.NewGuid()}_{prodReq.Archivo.FileName}";

            Producto nuevoProducto = new Producto
            {
                Nombre = prodReq.Nombre,
                Ruta = relativePath
            };

            await _unitOfWork.RepoProd.InsertAsync(nuevoProducto);

            if (await _unitOfWork.SaveAsync())
            {
                await StoreProductoAsync(relativePath, prodReq.Archivo);
            }

            return nuevoProducto;
        }

        public async Task<Producto> UpdateAsync(long id, CreateUpdateProductoRequest prodReq)
        {
            Producto prod = await _unitOfWork.RepoProd.GetByIdAsync(id);
            prod.Nombre = prodReq.Nombre;

            _unitOfWork.RepoProd.Update(prod);

            if (await _unitOfWork.SaveAsync() && prodReq.Archivo != null)
            {
                await StoreProductoAsync(prod.Ruta, prodReq.Archivo);
            }

            return prod;
        }

        public async Task DeleteAsync(long id)
        {
            Producto prod = await _unitOfWork.RepoProd.GetByIdAsync(id);
            _unitOfWork.RepoProd.Delete(prod);

            await _unitOfWork.SaveAsync();
        }

        private async Task StoreProductoAsync(string relativePath, IFormFile file)
        {
            using Stream stream = file.OpenReadStream();

            await FileHelper.SaveAsync(stream, relativePath);
        }
    }
}
