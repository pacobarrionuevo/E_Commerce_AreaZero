namespace E_Commerce_VS.Models.Database.Entidades
{
    public class ProductoCarrito
    {
        public int Id { get; set; }

        public long? ProductoId { get; set; }
        public int? CarritoId { get; set; }  // Puede ser nulo para carritos anónimos
        public int? Cantidad { get; set; }

        // Relaciones de claves foráneas
        public Carrito Carrito { get; set; }
        public Producto Producto { get; set; }
    }
}
