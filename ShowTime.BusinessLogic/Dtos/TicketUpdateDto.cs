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
    [Required(ErrorMessage = "Festival is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a festival")]
    public int FestivalId { get; set; }

    [Required(ErrorMessage = "Ticket type is required")]
    public TicketType? Type { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }

    public bool IsAvailable { get; set; }
}