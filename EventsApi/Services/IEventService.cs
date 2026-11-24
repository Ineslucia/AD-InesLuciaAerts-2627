using EventApi.Dtos;

namespace EventApi.Services
{
    public interface IMovieService
    {
        Task<List<EventDto>> GetAllAsync(EventQueryDto query);
        Task<EventDetailDto?> GetByIdAsync(int id);
        Task<EventDto?> CreateAsync(EventInputDto dto);
        Task<bool> UpdateAsync(int id, EventInputDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<TicketDto>?> GetReviewsAsync(int eventId);
        Task<TicketDto?> AddTicketAsync(int eventId, TicketInputDto dto);
    }
}
