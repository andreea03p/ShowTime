using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Dtos;

public class LineupUpdateDto
{
    [Required(ErrorMessage = "Stage is required.")]
    [StringLength(20)]
    public string Stage { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start date is required.")]
    [DataType(DataType.DateTime)]
    public DateTimeOffset StartTime { get; set; } = DateTime.Now;
}