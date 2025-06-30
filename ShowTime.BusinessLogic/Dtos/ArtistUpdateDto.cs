using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Dtos;

public class ArtistUpdateDto    
{
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
}

