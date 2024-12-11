namespace E_Commerce_VS.Models.Dto
{
    public class ProductCreateDto
    {
        public string Nombre { get; set; }
        public string Ruta { get; set; }
        public string Descripcion { get; set; }
        public int Precio { get; set; }
        public int Stock { get; set; }
    }
}
