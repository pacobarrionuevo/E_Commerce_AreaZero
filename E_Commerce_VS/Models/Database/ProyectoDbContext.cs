using Microsoft.EntityFrameworkCore;
using E_Commerce_VS.Models.Database.Entidades;
using Microsoft.Extensions.Options;

namespace E_Commerce_VS.Models.Database
{
    public class ProyectoDbContext : DbContext
    {
        private const string DATABASE_PATH = "areaZero.db";

        private readonly Settings _settings;

        //Tablas de la base de datos

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }

        // Configuramos el EntityFramework para crear un archivo de BBDD Sqlite

        public ProyectoDbContext(IOptions<Settings> options)
        {
            _settings = options.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            //optionsBuilder.UseSqlite($"DataSource={baseDir}{DATABASE_PATH}");
            optionsBuilder.UseSqlite(_settings.DatabaseConnection);
        }
    }
}
