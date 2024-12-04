﻿using Microsoft.EntityFrameworkCore;
using E_Commerce_VS.Models.Database.Entidades;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting.Server;

namespace E_Commerce_VS.Models.Database
{
    public class ProyectoDbContext : DbContext
    {
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
            //string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            //optionsBuilder.UseSqlite($"DataSource={baseDir}{DATABASE_PATH}");
            //optionsBuilder.UseMySql(ServerVersion.AutoDetect("Server = db10822.databaseasp.net; Database = db10822; Uid = db10822; Pwd = N + d9wJ5#4_Yt;"));
            string stringConnection = "Server = db10822.databaseasp.net; Database = db10822; Uid = db10822; Pwd = N+d9wJ5#4_Yt;";

#if DEBUG
                optionsBuilder.UseSqlite(_settings.DatabaseConnection);
#else

            optionsBuilder.UseMySql(stringConnection, ServerVersion.AutoDetect(stringConnection));
            #endif







        }
    }
}