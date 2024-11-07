namespace E_Commerce_VS.Models.Dto
{
    public class CreateUpdateProductoRequest
    {
        public string Nombre { get; set; }
        public IFormFile Archivo { get; set; }
        public int Precio { get; set; }
    }
}
