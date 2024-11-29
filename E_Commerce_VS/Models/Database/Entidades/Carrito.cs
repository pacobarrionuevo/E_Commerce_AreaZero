namespace E_Commerce_VS.Models.Database.Entidades
{
    public class Carrito
    {
        public int Id { get; set; }
        public int? UserId { get; set; }

        public ICollection<ProductoCarrito> ProductoCarrito { get; set; }
    }
}
