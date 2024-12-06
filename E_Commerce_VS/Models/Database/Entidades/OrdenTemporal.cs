namespace E_Commerce_VS.Models.Database.Entidades
{
    public class OrdenTemporal
    {
        public long Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public int? UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public List<ProductoCarrito> Productos { get; set; }
    }
}
