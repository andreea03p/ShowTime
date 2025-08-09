using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Dtos;

public class LineupGetDto
{
    public int FestivalId { get; set; }
    public int ArtistId { get; set; }
    public string Stage { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }

    public FestivalGetDto Festival { get; set; } = new();
    public ArtistGetDto Artist { get; set; } = new();
}