using E_Commerce_VS.Models.Database.Entidades;
using E_Commerce_VS.Models.Dto;
using E_Commerce_VS.Models.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.ML;

namespace E_Commerce_VS.Services
{
    public class ReviewService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly PredictionEnginePool<ModelInput, ModelOutput> _model;

        public ReviewService(UnitOfWork unitOfWork, PredictionEnginePool<ModelInput, ModelOutput> model)
        {
            _unitOfWork = unitOfWork;
            _model = model;
        }

        public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync()
        {
            // Obtener todas las reseñas desde el repositorio del UnitOfWork
            var reviews = await _unitOfWork.RepoRev.GetAllAsync();

            // Mapear manualmente a DTO
            var reviewDtos = new List<ReviewDto>();
            foreach (var review in reviews)
            {
                reviewDtos.Add(new ReviewDto
                {
                    Id = review.Id,
                    FechaPublicacion = review.FechaPublicacion,
                    TextReview = review.TextReview,
                    Label = review.Label,
                    UsuarioId = review.UsuarioId,
                    ProductoId = review.ProductoId
                });
            }

            return reviewDtos;
        }

        public async Task AddReviewAsync(CreateReviewDto createReviewDto)
        {
            var input = new ModelInput { Text = createReviewDto.TextReview };
            var prediction = _model.Predict(input);
            var label = (int)prediction.PredictedLabel;

            // Crear entidad Review desde el DTO
            var review = new Review
            {
                UsuarioId = createReviewDto.UsuarioId,
                FechaPublicacion = createReviewDto.FechaPublicacion,
                TextReview = createReviewDto.TextReview,
                Label = label,
                ProductoId = createReviewDto.ProductoId
            };

            // Insertar en el repositorio
            await _unitOfWork.RepoRev.InsertAsync(review);

            // Guardar cambios
            await _unitOfWork.SaveAsync();
        }

        public double CalculateAverageScore(IEnumerable<Review> reviews)
        {
            if (!reviews.Any())
            {
                return 5.0; // Valor neutral en la escala 1-10
            }

            // Convertir las etiquetas -1, 0, 1 a una escala de 1 a 10
            var totalScore = reviews.Sum(r => r.Label); // Sumar todas las etiquetas
            var averageLabel = totalScore / (double)reviews.Count();

            // Transformar la media de etiquetas a una escala de 1 a 10
            var averageScore = ((averageLabel + 1) / 2) * 9 + 1;

            return averageScore;
        }
    }
}

