using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "user name is required")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "email is required")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "password is required")]
    [StringLength(16, MinimumLength = 6, ErrorMessage = "password must be between 6 and 16 characters")]
    public string Password { get; set; } = string.Empty;
}