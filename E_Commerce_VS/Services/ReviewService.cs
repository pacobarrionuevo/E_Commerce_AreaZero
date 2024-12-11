using E_Commerce_VS.Models.Database.Entidades;
using E_Commerce_VS.Models.Dto;
using E_Commerce_VS.Models.Database;
using System.Collections.Generic;
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

        //Pilla todas las reviews y las mappea
        public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync()
        {
            var reviews = await _unitOfWork.RepoRev.GetAllAsync();

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

        //Crea una review : D
        public async Task<ReviewDto> AddReviewAsync(CreateReviewDto createReviewDto)
        {
            var input = new ModelInput { Text = createReviewDto.TextReview };
            var prediction = _model.Predict(input);
            var label = (int)prediction.PredictedLabel;

            var review = new Review
            {
                UsuarioId = createReviewDto.UsuarioId,
                FechaPublicacion = DateTime.UtcNow,
                TextReview = createReviewDto.TextReview,
                Label = label,
                ProductoId = createReviewDto.ProductoId
            };

            await _unitOfWork.RepoRev.InsertAsync(review);
            await _unitOfWork.SaveAsync();

            return new ReviewDto
            {
                Id = review.Id,
                UsuarioId = review.UsuarioId,
                ProductoId = review.ProductoId,
                FechaPublicacion = review.FechaPublicacion,
                TextReview = review.TextReview,
                Label = review.Label
            };
        }
    }
}
