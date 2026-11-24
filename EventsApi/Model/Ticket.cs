
namespace EventsApi.Model
{
    public class Ticket
    {
        public int Id { get; set; }
        public string HolderName { get; set; } = "";
        public string HolderEmail { get; set; } = "";
        public string TicketType { get; set; } = "";
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int EventId { get; set; }
        public Event? Event { get; set; }
    }
}
