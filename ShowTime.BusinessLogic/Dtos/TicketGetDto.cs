using ShowTime.DataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Dtos;

public class TicketGetDto
{
    public int Id { get; set; }
    public int FestivalId { get; set; }
    public TicketType Type { get; set; }
    public decimal Price { get; set; }
    public int TicketsTotal { get; set; }
    public int TicketsAvailable { get; set; }
    public bool IsAvailable { get; set; }
}