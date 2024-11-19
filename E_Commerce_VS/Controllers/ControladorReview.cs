using E_Commerce_VS.Models.Dto;
using E_Commerce_VS.Services;
using Microsoft.AspNetCore.Mvc;
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
            return Ok(reviews);
        }

        [HttpPost]
        public async Task<ActionResult> AddReview([FromBody] CreateReviewDto reviewDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _reviewService.AddReviewAsync(reviewDto);
            return CreatedAtAction(nameof(GetAllReviews), new { }, reviewDto);
        }
    }
}
