using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowTime.DataAccess.Models.Extras;


namespace ShowTime.DataAccess.Models;

public class Artist
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public Genres Genre { get; set; } = Genres.Pop;


    public ICollection<Lineup> Lineups { get; set; } = new List<Lineup>(); 
    public ICollection<Festival> Festivals { get; set; } = new List<Festival>();
}