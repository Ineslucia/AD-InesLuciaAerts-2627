using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventsApi.Dtos;
using EventsApi.Services;

namespace EventsApi.Controllers
{
    [ApiController]
    [Route("events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _service;

        public EventsController(IEventService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] EventQueryDto query)
        {
            var events = await _service.GetAllAsync(query);
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var event = await _service.GetByIdAsync(id);
            if (event is null)
            {
            return NotFound();
        }

            return Ok(event);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(EventInputDto dto)
        {
            var created = await _service.CreateAsync(dto);
            if (created is null)
            {
                return Problem(
                        title: "Er bestaat al een event met deze titel",
                        statusCode: StatusCodes.Status409Conflict
                    );
            }
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EventInputDto dto)
        {
            bool success = await _service.UpdateAsync(id, dto);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool success = await _service.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
