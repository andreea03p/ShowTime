using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Dtos;

public class LineupCreateDto
{
    public int ArtistId { get; set; }
    public int FestivalId { get; set; }
    public string Stage { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
}
