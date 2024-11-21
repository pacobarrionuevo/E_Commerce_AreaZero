namespace E_Commerce_VS.Models.Database.Entidades
{
    public class ProductoCarrito
    {
        public int Id { get; set; }

        public long? ProductoId { get; set; }
        public int? CarritoId { get; set; }
        public int? Cantidad { get; set; }

        
        public Carrito Carrito { get; set; }
        public Producto Producto { get; set; }
    }
}
