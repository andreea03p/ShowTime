using ShowTime.DataAccess.Models.Extras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Dtos;

public class BookingGetDto
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public int Quantity { get; set; }
    public Status BookingStatus { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string FestivalName { get; set; } = string.Empty;
    public string TicketType { get; set; } = string.Empty;
    public decimal TicketPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime FestivalDate { get; set; }
}