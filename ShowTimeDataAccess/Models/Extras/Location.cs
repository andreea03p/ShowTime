using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.DataAccess.Models.Extras;

public class Location
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public string Address { get; set; } = string.Empty;
}