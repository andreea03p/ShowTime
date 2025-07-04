using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Dtos;

public class ArtistCreateDto
{
    [Required(ErrorMessage = "Artist name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Genre is required.")]
    [StringLength(20, ErrorMessage = "Genre cannot have more than 20 characters.")]
    public string Genre { get; set; } = string.Empty;

    [Required(ErrorMessage = "URL is required.")]
    [Url(ErrorMessage = "Provide a valid URL for the image.")]
    public string Image { get; set; } = string.Empty;

}
