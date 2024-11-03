using E_Commerce_VS.Models.Database.Entidades;

namespace E_Commerce_VS.Models.Database.Repositories
{
    public class RepositorioProducto : Repositorio<Producto>
    {
        public RepositorioProducto(ProyectoDbContext context) : base(context)
        {
        }
    }
}
