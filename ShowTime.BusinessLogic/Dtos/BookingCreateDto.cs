using ShowTime.DataAccess.Models.Enums;
using ShowTime.DataAccess.Models.Extras;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Dtos;
public class BookingCreateDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Ticket ID must be greater than 0")]
    public int TicketId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "User ID must be greater than 0")]
    public int UserId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }
}