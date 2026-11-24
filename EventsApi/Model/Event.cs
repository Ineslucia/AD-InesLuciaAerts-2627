
using System.Net.Sockets;

namespace EventsApi.Model
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Location { get; set; } = "";
        public DateTime EventDate { get; set; }
        public int Capacity { get; set; }

        public List<Ticket> Tickets { get; set; } = new();
    }
}
