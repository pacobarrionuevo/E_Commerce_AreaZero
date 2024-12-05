using E_Commerce_VS.Models.Database.Entidades;
using E_Commerce_VS.Models.Dto;
using E_Commerce_VS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce_VS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorReview : ControllerBase
    {
        private readonly ReviewService _reviewService;

        public ControladorReview(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // GET: api/ControladorReview
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAllReviews()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            var averageScore = _reviewService.CalculateAverageScore(reviews.Select(r => new Review
            {
                Id = r.Id,
                FechaPublicacion = r.FechaPublicacion,
                TextReview = r.TextReview,
                Label = r.Label,
                UsuarioId = r.UsuarioId,
                ProductoId = r.ProductoId
            }));

            return Ok(new { reviews, averageScore });
        }

        // Proteger este método con [Authorize]
        [Authorize]
        [HttpPost]
        
        public async Task<ActionResult> AddReview([FromBody] CreateReviewDto reviewDto)
        {
            await _reviewService.AddReviewAsync(reviewDto);
            return CreatedAtAction(nameof(GetAllReviews), new { }, reviewDto);
        }
    }
}
