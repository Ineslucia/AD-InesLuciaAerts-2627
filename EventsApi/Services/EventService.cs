using EventApi.Services;
using EventApi.Data;
using EventApi.Dtos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EventApi.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _repository;

        public EventService(IEventRepository repository)
        {
            _repository = repository;
        }

        public async Task<TicketDto?> AddTicketAsync(int eventId, TicketInputDto dto)
        {
            if (!await _repository.ExistsAsync(eventId))
            {
                return null;
            }

            var ticket = dto.ToEntity();
            ticket.EventId = eventId;
            await _repository.AddTicketAsync(ticket);
            return ticket.ToDto();
        }

        public async Task<EventDto?> CreateAsync(EventInputDto dto)
        {
            // business regel: geen dubbele titels
            if (await _repository.ExistsByTitleAsync(dto.Title))
            {
                return null;
            }

            var event = dto.ToEntity();
            await _repository.AddAsync(event);
            return event.ToDto();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var event = await _repository.GetByIdAsync(id);
            if (event is null)
            {
                return false;
            }

            await _repository.DeleteAsync(event);
            return true;
        }

        public async Task<List<EventDto>> GetAllAsync(EventQueryDto query)
        {
            var events = await _repository.GetAllAsync(query);
            return events.Select(e => e.ToDto()).ToList();

        }

        public async Task<EventDetailDto?> GetByIdAsync(int id)
        {
            var event = await _repository.GetByIdAsync(id);
            return Event?.ToDetailDto();
        }

        public async Task<List<TicketDto>?> GetReviewsAsync(int eventId)
        {
            if (!await _repository.ExistsAsync(eventId))
            {
                return null;
            }

            var tickets = await _repository.GetTicketsAsync(eventId);
            return tickets.Select(t => t.ToDto()).ToList();
        }

        public async Task<bool> UpdateAsync(int id, EventInputDto dto)
        {
            var event = await _repository.GetByIdAsync(id);
            if (event is null)
            {
                return false;
            }

            event.Title = dto.Title;
            event.Year = dto.Year;
            await _repository.UpdateAsync(event);
            return true;
        }
    }
}
