using ShowTime.DataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Dtos;

public class LoginResponseDto
{
    public int UserId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public Role Role { get; set; }
}