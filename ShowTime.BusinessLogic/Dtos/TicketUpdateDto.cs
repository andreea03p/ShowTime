using ShowTime.DataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Dtos;

public class TicketUpdateDto
{
    [Required]
    public int FestivalId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public TicketType Type { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public bool IsAvailable { get; set; } = true;
}