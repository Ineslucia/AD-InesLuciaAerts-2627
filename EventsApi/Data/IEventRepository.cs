using Microsoft.AspNetCore.Mvc.ViewEngines;
using EventApi.Dtos;
using EventApi.Model;
using EventsApi.Model;

namespace EventApi.Data
{
    public interface IEventRepository
    {
        Task<List<Event>> GetAllAsync(EventQueryDto query);
        Task<Event?> GetByIdAsync(int id);
        Task AddAsync(Event event);

        Task UpdateAsync(Event event);

        Task DeleteAsync(Event event);

        Task<bool> ExistsByTitleAsync(string title);
        Task<bool> ExistsAsync(int id);
        Task<List<Ticket>> GetTicketsAsync(int eventId);
        Task AddReviewAsync(Ticket ticket);
    }
}
