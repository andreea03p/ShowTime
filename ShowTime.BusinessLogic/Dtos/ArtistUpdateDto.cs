using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ShowTime.DataAccess.Models.Extras;

namespace ShowTime.BusinessLogic.Dtos;

public class ArtistUpdateDto    
{
    [Required(ErrorMessage = "Artist name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Genre is required.")]
    public Genres Genre { get; set; }

    [Required(ErrorMessage = "URL is required.")]
    [Url(ErrorMessage = "Provide a valid URL for the image.")]
    public string Image { get; set; } = string.Empty;
}

