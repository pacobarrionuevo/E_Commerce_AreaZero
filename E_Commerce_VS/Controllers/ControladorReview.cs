using E_Commerce_VS.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using E_Commerce_VS.Services;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAllReviews()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }

        // Para crear una review a un producto, tenemos que asociarla con un usuarioId, asi que necesita el authorize
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ReviewDto>> AddReview([FromBody] CreateReviewDto reviewDto)
        {
            var newReview = await _reviewService.AddReviewAsync(reviewDto);
            return CreatedAtAction(nameof(GetAllReviews), new { id = newReview.Id }, newReview);
           
        }
    }
}
