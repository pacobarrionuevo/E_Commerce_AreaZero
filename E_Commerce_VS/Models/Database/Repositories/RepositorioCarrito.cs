using E_Commerce_VS.Models.Database.Entidades;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_VS.Models.Database.Repositories
{
    public class RepositorioCarrito: Repositorio<ProductoCarrito>
    {
        private readonly ProyectoDbContext _dbContext;
        public RepositorioCarrito(ProyectoDbContext context) : base(context) {
            _dbContext = context;
        }

        public async Task<Carrito> GetCarritoByUserIdAsync(int userId)
        {
            return await _dbContext.Carritos
                .Include(c => c.ProductoCarrito) 
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

    }
}
