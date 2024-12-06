using E_Commerce_VS.Models.Database.Repositories;
using E_Commerce_VS.Models.Database;

public class UnitOfWork
{
    private readonly ProyectoDbContext _context;

    public RepositorioProducto RepoProd { get; init; }
    public RepositorioReview RepoRev { get; init; }
    public RepositorioCarrito RepoCar { get; init; }

    // Exponer el DbContext
    public ProyectoDbContext Context => _context;

    public UnitOfWork(ProyectoDbContext context, RepositorioProducto repoProd, RepositorioReview repoRev, RepositorioCarrito repoCar)
    {
        _context = context;

        RepoProd = repoProd;
        RepoRev = repoRev;
        RepoCar = repoCar;
    }

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
