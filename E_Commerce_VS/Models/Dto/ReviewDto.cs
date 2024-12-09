using System.Text.Json.Serialization;

namespace E_Commerce_VS.Models.Dto
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public string TextReview { get; set; }
        public int Label { get; set; }

        public int UsuarioId { get; set; }
        public long ProductoId { get; set; }
    }
}