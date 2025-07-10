    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowTime.DataAccess.Models.Extras;

namespace ShowTime.DataAccess.Models;

public class Lineup
{
    public int FestivalId { get; set; }
    public int ArtistId { get; set; }
    public DateTimeOffset StartTime { get; set; } = DateTimeOffset.Now;
    public string Stage { get; set; } = string.Empty;

    public virtual Festival Festival { get; set; } = new Festival
    {
        Location = new Location()
    };
    public virtual Artist Artist { get; set; } = new Artist();
}