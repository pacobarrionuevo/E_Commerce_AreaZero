using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace E_Commerce_VS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControladorImagen : ControllerBase
    {
        [HttpGet("{imageName}")]
        public IActionResult GetImage(string imageName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { Message = $"Image '{imageName}' not found at '{filePath}'." });
            }
            var image = System.IO.File.OpenRead(filePath);
            return File(image, "image/png"); 
        }
    }
}
