using EventsApi.Data;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using EventApi.Dtos;
using EventApi.Model;

namespace EventApi.Data
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Event>> GetAllAsync(EventQueryDto query)
        {
            return await _context.Events
                                .Include(e => e.Tickets)
                                .ApplyQuery(query)
                                .ToListAsync();
        }


        public async Task<Event?> GetByIdAsync(int id)
        {
            return await _context.Events
                            .Include(e => e.Tickets)
                            .FirstOrDefaultAsync(e=> e.Id == id);
        }

        public async Task AddAsync(Event event)
        {
            _context.Events.Add(event);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Event event)
        {
            _context.Events.Update(event);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(Event event)
        {
            _context.Events.Remove(event);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Events.AnyAsync(e => e.Id == id);
        }

        public async Task<List<Ticket>> GetReviewsAsync(int eventId)
        {
            return await _context.Ticket
                            .Where(t => t.eventId == eventId)
                            .ToListAsync();
        }

        public async Task AddReviewAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsByTitleAsync(string title)
        {
            return await _context.Events.AnyAsync(e => e.Title == title);
        }
    }
}