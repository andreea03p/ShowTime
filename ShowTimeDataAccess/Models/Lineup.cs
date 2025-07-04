﻿    using System;
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
    public DateTime StartDate { get; set; }
    public string Stage { get; set; } = string.Empty;

    public Festival Festival { get; set; } = new Festival
    {
        Location = new Location()
    };
    public Artist Artist { get; set; } = new Artist();
}