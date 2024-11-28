using E_Commerce_VS.Models.Database.Entidades;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_VS.Models.Database.Repositories
{
    public class RepositorioCarrito: Repositorio<ProductoCarrito>
    {
        public RepositorioCarrito(ProyectoDbContext context) : base(context) {}
    }
}
