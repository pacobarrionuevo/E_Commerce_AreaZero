namespace E_Commerce_VS.Models.Database.Entidades
{
    public class Producto
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Ruta { get; set; }
        public int Precio { get; set; }
        public int Stock { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();

    }
}