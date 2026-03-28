using Microsoft.AspNetCore.Mvc;
using EventBooking.API.Data;
using EventBooking.API.Models;

namespace EventBooking.API.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetEvents()
        {
            return Ok(_context.Events.ToList());
        }
    }
}