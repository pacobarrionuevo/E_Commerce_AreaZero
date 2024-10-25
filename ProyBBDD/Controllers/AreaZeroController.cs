using Microsoft.AspNetCore.Mvc;
using E_Commerce_VS.Database.Entidades;
using Microsoft.EntityFrameworkCore;
using E_Commerce_VS.Database;

namespace E_Commerce_VS.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AreaZeroController : ControllerBase
    {
        private ProyectoDbContext _dbContext;

        public AreaZeroController(ProyectoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //Devolver todos los usuarios
        [HttpGet]
        public IEnumerable<Usuario> GetUsuarios()
        {
            return _dbContext.Usuarios;
        }


    }
}
