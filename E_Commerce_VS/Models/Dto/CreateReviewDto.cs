namespace E_Commerce_VS.Models.Dto
{
    public class CreateReviewDto
    {
        public int UsuarioId { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public string TextReview { get; set; }
        public long ProductoId { get; set; }
    }
}