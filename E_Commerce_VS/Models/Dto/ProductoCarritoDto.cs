namespace E_Commerce_VS.Models.Dto
{
    public class ProductoCarritoDto
    {
        public long ProductoId { get; set; }
        public string Nombre { get; set; }
        public string Ruta { get; set; }
        public int Precio { get; set; }
        public int Cantidad { get; set; }
    }
}
