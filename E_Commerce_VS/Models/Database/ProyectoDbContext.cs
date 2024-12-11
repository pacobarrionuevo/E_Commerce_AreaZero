using Microsoft.EntityFrameworkCore;
using E_Commerce_VS.Models.Database.Entidades;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting.Server;

namespace E_Commerce_VS.Models.Database
{
    public class ProyectoDbContext : DbContext
    {
        //Nombre de la base de datos y luego se llama ahi
        private const string DATABASE_PATH = "areaZero.db";

        private readonly Settings _settings;

        //Tablas de la base de datos

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<ProductoCarrito> ProductoCarritos { get; set; }

        public DbSet<OrdenTemporal> OrdenesTemporales { get; set; }

        public DbSet<Review> Reviews { get; set; }

        // Configuramos el EntityFramework para crear un archivo de BBDD Sqlite

        public ProyectoDbContext(IOptions<Settings> options)
        {
            _settings = options.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string stringConnection = "Server=db11254.databaseasp.net; Database=db11254; Uid=db11254; Pwd=B_n98L=yHd5?; ";


            #if DEBUG
                optionsBuilder.UseSqlite(_settings.DatabaseConnection);
            #else
                optionsBuilder.UseMySql(stringConnection, ServerVersion.AutoDetect(stringConnection));
            #endif
        }
    }
}