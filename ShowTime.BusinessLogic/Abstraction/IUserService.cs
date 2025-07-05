using ShowTime.BusinessLogic.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Abstraction;

public interface IUserService
{
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    //Task<LoginResponseDto> RegisterAsync(LoginDto loginDto);
}