﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "email is required")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "password is required")]
    public string Password { get; set; } = string.Empty;
}