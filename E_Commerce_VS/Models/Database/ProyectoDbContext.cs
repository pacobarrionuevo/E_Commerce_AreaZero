using Microsoft.EntityFrameworkCore;
using E_Commerce_VS.Models.Database.Entidades;

namespace E_Commerce_VS.Models.Database
{
    public class ProyectoDbContext : DbContext
    {
        private const string DATABASE_PATH = "areaZero.db";

        //Tablas de la base de datos
        // IREMOS AÑADIENDO POCO A POCO, AHORA MISMO SÓLO HACE FALTA LA DE USUARIO PARA EL INICIO DE SESIÓN

        public DbSet<Usuario> Usuarios { get; set; }

        // Configuramos el EntityFramework para crear un archivo de BBDD Sqlite

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            optionsBuilder.UseSqlite($"DataSource={baseDir}{DATABASE_PATH}");
        }
    }
}
