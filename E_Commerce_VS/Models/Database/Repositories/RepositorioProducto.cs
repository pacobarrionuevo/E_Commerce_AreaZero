using E_Commerce_VS.Models.Database.Entidades;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_VS.Models.Database.Repositories
{
    public class RepositorioProducto : Repositorio<Producto>
    {
        public RepositorioProducto(ProyectoDbContext context) : base(context)
        {
        }

        public async Task<ICollection<Producto>> GetAllWithFullDataSync()
        {
            return await GetQueryable()
                .Include(Producto => Producto.Reviews)
                .ToArrayAsync();
        }
    }
}
