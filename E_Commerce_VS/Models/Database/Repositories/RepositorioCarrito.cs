using E_Commerce_VS.Models.Database.Entidades;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_VS.Models.Database.Repositories
{
    public class RepositorioCarrito : Repositorio<Carrito>
    {
        private readonly ProyectoDbContext _dbContext;

        public RepositorioCarrito(ProyectoDbContext context) : base(context)
        {
            _dbContext = context;
        }

        // Para obtener todos los carritos que hay guardados incluyendo sus productos
        public async Task<IEnumerable<Carrito>> GetAllAsync()
        {
            return await _dbContext.Carritos
                .Include(c => c.ProductoCarrito)
                .ToListAsync();
        }

        // Este metodo devuelve el carrito asociado a un usuario (por su Id)
        public async Task<Carrito> GetCarritoByUserIdAsync(int? userId)
        {
            return await _dbContext.Carritos
                .Include(c => c.ProductoCarrito)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}