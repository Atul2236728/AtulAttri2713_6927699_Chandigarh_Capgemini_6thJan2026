using Microsoft.AspNetCore.Mvc;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiHealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                version = "1.0.0",
                endpoints = new[]
                {
                    "GET  /api/products",
                    "GET  /api/products/{id}",
                    "POST /api/products",
                    "PUT  /api/products/{id}",
                    "DELETE /api/products/{id}"
                }
            });
        }
    }
}
