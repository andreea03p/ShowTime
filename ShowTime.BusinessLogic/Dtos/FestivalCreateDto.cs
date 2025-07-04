using ShowTime.DataAccess.Models.Extras;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Dtos;

public class FestivalCreateDto
{
    [Required(ErrorMessage = "Festival name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start date is required.")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = new DateTime(2025, 1, 1);

    [Required(ErrorMessage = "End date is required.")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; } = new DateTime(2028, 1, 1);

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(200, ErrorMessage = "Address cannot have more than 100 characters")]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Latitude is required.")]
    [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
    public double Latitude { get; set; }

    [Required(ErrorMessage = "Longitude is required.")]
    [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
    public double Longitude { get; set; }

    [Required(ErrorMessage = "SplashArt is required.")]
    [Url(ErrorMessage = "Provide a valid URL for the splash art.")]
    public string? SplashArt { get; set; }

    [Required(ErrorMessage = "Capacity is required.")]
    [Range(1, 1000000, ErrorMessage = "Capacity must be between 1 and 10000000.")]
    public int Capacity { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndDate <= StartDate)
        {
            yield return new ValidationResult(
                "End date must be after start date!",
                new[] { nameof(EndDate) });
        }
    }
}
