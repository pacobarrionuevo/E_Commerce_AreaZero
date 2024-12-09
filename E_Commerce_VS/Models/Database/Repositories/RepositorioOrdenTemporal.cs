using E_Commerce_VS.Models.Database.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace E_Commerce_VS.Models.Database.Repositories
{
    public class RepositorioOrdenTemporal : Repositorio<OrdenTemporal>
    {
        public RepositorioOrdenTemporal(ProyectoDbContext context) : base(context)
        {
        }


    }
}
