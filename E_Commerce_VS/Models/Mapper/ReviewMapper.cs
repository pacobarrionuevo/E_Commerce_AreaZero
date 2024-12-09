using E_Commerce_VS.Models.Database.Entidades;
using E_Commerce_VS.Models.Dto;

namespace E_Commerce_VS.Models.Mapper
{
    public class ReviewMapper
    {
        public ReviewDto ToDto(Review review)
        {
            return new ReviewDto()
            {
                Id = review.Id,
                FechaPublicacion = review.FechaPublicacion,
                TextReview = review.TextReview,
                Label = review.Label,
                UsuarioId = review.UsuarioId,
                ProductoId = review.ProductoId
            };
        }

        public IEnumerable<ReviewDto> ToDto(IEnumerable<Review> reviews)
        {
            return reviews.Select(review => ToDto(review));
        }
    }
}