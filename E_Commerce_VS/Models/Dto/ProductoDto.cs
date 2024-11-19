namespace E_Commerce_VS.Models.Dto
{
    public class ProductoDto
    {
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Ruta { get; set; }
        public int Precio { get; set; }
        public int Stock { get; set; }
    }
}
