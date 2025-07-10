using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowTime.DataAccess.Models.Extras;

namespace ShowTime.DataAccess.Models;

public class Festival
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = new DateTime(2028, 1, 1);
    public required Location Location { get; set; }
    public string SplashArt { get; set; } = string.Empty;
    public int Capacity { get; set; }

    public virtual ICollection<Lineup> Lineups { get; set; } = new List<Lineup>();
    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
