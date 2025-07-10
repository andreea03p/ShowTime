using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Dtos;

public class LineupCreateDto
{
    [Required]
    public int FestivalId { get; set; }

    [Required]
    public int ArtistId { get; set; }

    [Required]
    [StringLength(100)]
    public string Stage { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start date is required.")]
    [DataType(DataType.DateTime)]
    public DateTime? StartTime { get; set; } = DateTime.Now;
}
