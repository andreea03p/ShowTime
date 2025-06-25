using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTimeDataAccess.Models;

public class Lineup
{
    public int FestivalId { get; set; }
    public int ArtistId { get; set; }
    public DateTime StartDate { get; set; }
    public string Stage { get; set; } = string.Empty;

    public Festivals Festival { get; set; } = new Festivals();
    public Artist Artist { get; set; } = new Artist();

}