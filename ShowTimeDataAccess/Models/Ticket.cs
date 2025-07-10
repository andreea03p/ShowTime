using ShowTime.DataAccess.Models.Enums;
using ShowTime.DataAccess.Models.Extras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.DataAccess.Models;

public class Ticket
{
    public int Id { get; set; }
    public int FestivalId { get; set; }
    public TicketType TicketType { get; set; }
    public decimal Price { get; set; }
    public int TicketsTotal { get; set; }
    public int TicketsAvailable { get; set; }
    public bool IsAvailable { get; set; } = true;

    public virtual Festival Festival { get; set; } = new Festival
    {
        Location = new Location()
    };
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
