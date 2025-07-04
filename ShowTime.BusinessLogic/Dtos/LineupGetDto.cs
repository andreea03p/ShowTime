using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Dtos;

public class LineupGetDto
{
    public string Stage { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
}
