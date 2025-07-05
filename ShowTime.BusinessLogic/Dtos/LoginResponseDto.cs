using ShowTime.DataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Dtos;

public class LoginResponseDto
{
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }
}
