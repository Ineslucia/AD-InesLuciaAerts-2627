using Microsoft.AspNetCore.Mvc;
using EventApi.Dtos;
using EventApi.Services;

namespace EventApi.Controllers
{
    [ApiController]
    [Route("events/{eventId}/tickets")]
    public class TicketsController : ControllerBase
    {
        private readonly IEventService _service;

        public TicketsController(IEventService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int eventId)
        {
            var tickets = await _service.GetReviewsAsync(eventId);
            if (tickets is null)
            {
                return NotFound();
            }
            return Ok(tickets);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int eventId, TicketInputDto dto)
        {
            var ticket = await _service.AddTicketAsync(eventId, dto);
            if (ticket is null)
            {
                return NotFound();
            }
            return Created($"/events/{eventId}/tickets/{ticket.Id}", ticket);

        }
    }
}
