using E_Commerce_VS.Models.Database.Repositories;

namespace E_Commerce_VS.Models.Database
{
    public class UnitOfWork
    {
        private readonly ProyectoDbContext _context;

        public RepositorioProducto RepoProd { get; init; }

        public UnitOfWork(ProyectoDbContext context, RepositorioProducto repoProd)
        {
            _context = context;

            RepoProd = repoProd;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
