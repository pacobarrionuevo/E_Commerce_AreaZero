namespace E_Commerce_VS.Models.Database.Entidades
{
    public class ProductoOrdenTemporal
    {
        public long Id { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; } 
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; } 
        public int OrdenTemporalId { get; set; }
        public OrdenTemporal OrdenTemporal { get; set; }
    }
}
