using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowTime.DataAccess.Models.Enums;
using ShowTime.DataAccess.Models.Extras;

namespace ShowTime.DataAccess.Models;

public class Booking
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public int Quantity { get; set; }
    public Status BookingStatus { get; set; } = Status.Pending;

    public virtual User User { get; set; } = new User();
    public virtual Ticket Ticket { get; set; } = new Ticket();
}
