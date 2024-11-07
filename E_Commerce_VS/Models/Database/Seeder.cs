using static System.Net.Mime.MediaTypeNames;
using E_Commerce_VS.Models.Database.Entidades;

namespace E_Commerce_VS.Models.Database
{
    public class Seeder
    {
        private readonly ProyectoDbContext _dbContext;

        public Seeder(ProyectoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedAsync()
        {
            await SeedImagesAsync();
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedImagesAsync()
        {
            Producto[] productos = new Producto[]
            {
        new Producto() { Id = 1, Nombre = "Alakazam EX Collection", Ruta = "images/1.png", Precio = 100 },
        new Producto() { Id = 2, Nombre = "League Battle Deck: Regieleki & Miraidon", Ruta = "images/2.png", Precio = 200 },
        new Producto() { Id = 3, Nombre = "Combined Powers: Lugia & Ho-Oh", Ruta = "images/3.png", Precio = 300 },
        new Producto() { Id = 4, Nombre = "Lucario VStar Premium Collection", Ruta = "images/4.png", Precio = 400 },
        new Producto() { Id = 5, Nombre = "Sword & Shield: Astral Radiance Pack", Ruta = "images/5.png", Precio = 500 },
        new Producto() { Id = 6, Nombre = "Sprigatito Paldea Collection", Ruta = "images/6.png", Precio = 600 },
        new Producto() { Id = 7, Nombre = "Carpeta Plástico GuardaCartas", Ruta = "images/7.png", Precio = 700 },
        new Producto() { Id = 8, Nombre = "Maletin GuardaCartas Charizard Theme", Ruta = "images/8.png", Precio = 800 },
        new Producto() { Id = 9, Nombre = "Pokemon Card Game Case", Ruta = "images/9.png", Precio = 900 },
        new Producto() { Id = 10, Nombre = "Sword & Shield: Lost Origin Pack", Ruta = "images/10.png", Precio = 1000 },
        new Producto() { Id = 11, Nombre = "Pikachu V Collection", Ruta = "images/11.png", Precio = 1100 },
        new Producto() { Id = 12, Nombre = "Paradox Powers EX: Koraidon", Ruta = "images/12.png", Precio = 1200 },
        new Producto() { Id = 13, Nombre = "Sword & Shield: Silver Tempest (Togetic)", Ruta = "images/13.png", Precio = 1300 },
        new Producto() { Id = 14, Nombre = "Baraja Combate EX: Iron Leaves & Tapu Koko", Ruta = "images/14.png", Precio = 1400 },
        new Producto() { Id = 15, Nombre = "Pokemon TCG Arceus Case", Ruta = "images/15.png", Precio = 1500 },
        new Producto() { Id = 16, Nombre = "Escarlata y Purpura: Mascara Crepuscular", Ruta = "images/16.png", Precio = 1600 },
        new Producto() { Id = 17, Nombre = "Sword & Shield: Silver Tempest", Ruta = "images/17.png", Precio = 1700 },
        new Producto() { Id = 18, Nombre = "Escarlata y Purpura: Llamas Obsidianas", Ruta = "images/18.png", Precio = 1800 },
        new Producto() { Id = 19, Nombre = "Escarlata y Purpura: Evoluciones En Paldea", Ruta = "images/19.png", Precio = 1900 },
        new Producto() { Id = 20, Nombre = "Sword & Shield: Evolving Skies", Ruta = "images/20.png", Precio = 2000 },
            };

            await _dbContext.Productos.AddRangeAsync(productos);
        }

    }
}

