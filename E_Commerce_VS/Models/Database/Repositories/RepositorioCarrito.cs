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

        // Obtener todos los carritos incluyendo sus productos
        public async Task<IEnumerable<Carrito>> GetAllAsync()
        {
            return await _dbContext.Carritos
                .Include(c => c.ProductoCarrito)
                .ToListAsync();
        }

        // Obtener un carrito por UserId incluyendo sus productos
        public async Task<Carrito> GetCarritoByUserIdAsync(int? userId)
        {
            return await _dbContext.Carritos
                .Include(c => c.ProductoCarrito)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
