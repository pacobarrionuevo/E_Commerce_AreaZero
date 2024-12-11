using static System.Net.Mime.MediaTypeNames;
using E_Commerce_VS.Models.Database.Entidades;
using E_Commerce_VS.Recursos;

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
                new Producto() { Id = 1, Nombre = "Alakazam EX Collection", Ruta = "images/1.png", Precio = 2750, Stock = 10, Descripcion = "Esta colección presenta al poderoso Alakazam EX. Incluye una carta promocional especial y varios sobres de expansión." },
                new Producto() { Id = 2, Nombre = "League Battle Deck: Regieleki & Miraidon", Ruta = "images/2.png", Precio = 2200, Stock = 10, Descripcion = "Este mazo de batalla incluye las potentes cartas de Regieleki y Miraidon. Perfecto para los duelos de la Liga Pokémon." },
                new Producto() { Id = 3, Nombre = "Combined Powers: Lugia & Ho-Oh", Ruta = "images/3.png", Precio = 1300, Stock = 10, Descripcion = "Combina las fuerzas de Lugia y Ho-Oh con este paquete exclusivo que incluye cartas y sobres de expansión temáticos." },
                new Producto() { Id = 4, Nombre = "Lucario VStar Premium Collection", Ruta = "images/4.png", Precio = 3400, Stock = 10, Descripcion = "La colección premium de Lucario VStar ofrece cartas exclusivas de Lucario y una selección de sobres de expansión." },
                new Producto() { Id = 5, Nombre = "Sword & Shield: Astral Radiance Pack", Ruta = "images/5.png", Precio = 499, Stock = 10, Descripcion = "Un paquete de expansión de Sword & Shield: Astral Radiance, lleno de nuevas cartas para tu colección." },
                new Producto() { Id = 6, Nombre = "Sprigatito Paldea Collection", Ruta = "images/6.png", Precio = 2750, Stock = 10, Descripcion = "La colección de Sprigatito de Paldea incluye cartas temáticas y sobres de expansión para los fans de este adorable Pokémon." },
                new Producto() { Id = 7, Nombre = "Carpeta Plástico GuardaCartas", Ruta = "images/7.png", Precio = 700, Stock = 10, Descripcion = "Una carpeta resistente y elegante para guardar y proteger tus cartas más preciadas." },
                new Producto() { Id = 8, Nombre = "Maletín GuardaCartas Charizard Theme", Ruta = "images/8.png", Precio = 1099, Stock = 10, Descripcion = "Un maletín temático de Charizard, perfecto para transportar y almacenar tu colección de cartas Pokémon." },
                new Producto() { Id = 9, Nombre = "Pokemon Card Game Case", Ruta = "images/9.png", Precio = 750, Stock = 10, Descripcion = "Un estuche compacto y resistente para organizar y proteger tu mazo de cartas Pokémon." },
                new Producto() { Id = 10, Nombre = "Sword & Shield: Lost Origin Pack", Ruta = "images/10.png", Precio = 499, Stock = 10, Descripcion = "Expande tu colección con el paquete de Sword & Shield: Lost Origin, que incluye nuevas y emocionantes cartas." },
                new Producto() { Id = 11, Nombre = "Pikachu V Collection", Ruta = "images/11.png", Precio = 1999, Stock = 10, Descripcion = "Celebra a Pikachu con esta colección especial que incluye cartas exclusivas y sobres de expansión." },
                new Producto() { Id = 12, Nombre = "Paradox Powers EX: Koraidon", Ruta = "images/12.png", Precio = 1200, Stock = 10, Descripcion = "Descubre las increíbles habilidades de Koraidon en esta colección EX con cartas únicas y sobres temáticos." },
                new Producto() { Id = 13, Nombre = "Sword & Shield: Silver Tempest (Togetic)", Ruta = "images/13.png", Precio = 2500, Stock = 10, Descripcion = "Un paquete de expansión que destaca a Togetic con nuevas cartas y sobres de Sword & Shield: Silver Tempest." },
                new Producto() { Id = 14, Nombre = "Baraja Combate EX: Iron Leaves & Tapu Koko", Ruta = "images/14.png", Precio = 2750, Stock = 10, Descripcion = "Mazo de combate EX que presenta a Iron Leaves y Tapu Koko, ideal para batallas estratégicas." },
                new Producto() { Id = 15, Nombre = "Pokemon TCG Arceus Case", Ruta = "images/15.png", Precio = 2500, Stock = 10, Descripcion = "Un estuche especial para guardar tus cartas de TCG con el tema de Arceus, manteniendo tu colección segura y organizada." },
                new Producto() { Id = 16, Nombre = "Escarlata y Púrpura: Máscara Crepuscular", Ruta = "images/16.png", Precio = 499, Stock = 10, Descripcion = "Paquete de expansión de Escarlata y Púrpura, que ofrece nuevas cartas temáticas para tu colección." },
                new Producto() { Id = 17, Nombre = "Sword & Shield: Silver Tempest", Ruta = "images/17.png", Precio = 499, Stock = 10, Descripcion = "Un paquete de expansión de Sword & Shield: Silver Tempest con cartas emocionantes y poderosas." },
                new Producto() { Id = 18, Nombre = "Escarlata y Púrpura: Llamas Obsidianas", Ruta = "images/18.png", Precio = 499, Stock = 10, Descripcion = "Expande tu mazo con el paquete de Escarlata y Púrpura: Llamas Obsidianas, que incluye cartas exclusivas." },
                new Producto() { Id = 19, Nombre = "Escarlata y Púrpura: Evoluciones en Paldea", Ruta = "images/19.png", Precio = 499, Stock = 10, Descripcion = "Nuevas cartas de Escarlata y Púrpura centradas en las evoluciones de Paldea, perfectas para fortalecer tu colección." },
                new Producto() { Id = 20, Nombre = "Sword & Shield: Evolving Skies", Ruta = "images/20.png", Precio = 499, Stock = 10, Descripcion = "Un paquete de expansión de Sword & Shield: Evolving Skies, lleno de cartas únicas y sorprendentes." },
            };
            Review[] reviews = new Review[]
            {
                new Review() {Id = 1, UsuarioId = 1, FechaPublicacion = DateTime.UtcNow , TextReview = "Me gusta mucho esta coleccion", Label = 1, ProductoId = 1},
                new Review() {Id = 2, UsuarioId = 2, FechaPublicacion = DateTime.UtcNow , TextReview = "Me ha parecido una coleccion malisima", Label = -1, ProductoId = 1},
                new Review() {Id = 3, UsuarioId = 3, FechaPublicacion = DateTime.UtcNow , TextReview = "Este producto me da igual", Label = 0, ProductoId = 2},
                new Review() {Id = 4, UsuarioId = 4, FechaPublicacion = DateTime.UtcNow , TextReview = "No me gusta", Label = -1, ProductoId = 3},
            };

            Usuario[] usuarios = new Usuario[] { 
                new Usuario() { UsuarioId = 1, Nombre = "Jose", Password = PasswordHelper.Hash("jose777"), Email = "jose@gmail.com", Direccion = "Calle Jose", esAdmin = true },
                new Usuario() { UsuarioId = 2, Nombre = "paco", Password = PasswordHelper.Hash("paco"), Email = "paco@gmail.com", Direccion = "Calle Paco", esAdmin = false } 
            };
            await _dbContext.Productos.AddRangeAsync(productos);
            await _dbContext.Reviews.AddRangeAsync(reviews);
            await _dbContext.Usuarios.AddRangeAsync(usuarios);
        }

    }
}
