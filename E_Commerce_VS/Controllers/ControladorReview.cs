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

        // GET: api/ControladorReview
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAllReviews()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }

        // Proteger este método con [Authorize]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddReview([FromBody] CreateReviewDto reviewDto)
        {
            await _reviewService.AddReviewAsync(reviewDto);
            return CreatedAtAction(nameof(GetAllReviews), new { }, reviewDto);
        }
    }
}
