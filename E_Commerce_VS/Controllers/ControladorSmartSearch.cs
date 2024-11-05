using E_Commerce_VS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_VS.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    public class ControladorSmartSearch : ControllerBase
    {
        [HttpGet]
    public IEnumerable<string> Search([FromQuery] string query)
    {
        SmartSearchService smartSearchService = new SmartSearchService();

        return smartSearchService.Search(query);
    }   
}

