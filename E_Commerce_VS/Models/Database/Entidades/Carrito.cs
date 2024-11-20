namespace E_Commerce_VS.Models.Database.Entidades
{
    public class Carrito
    {
        public int Id { get; set; }

        // El UserId puede ser nulo para carritos anónimos
        public int? UserId { get; set; }

        // Se puede mantener la relación de productos
        public ICollection<ProductoCarrito> ProductoCarrito { get; set; }
    }
}
