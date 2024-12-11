using E_Commerce_VS.Models.Database.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace E_Commerce_VS.Models.Database.Repositories
{
    //Repositorio Review 
    public class RepositorioReview : Repositorio<Review>
    {
        public RepositorioReview(ProyectoDbContext context) : base(context)
        {
        }


    }
}