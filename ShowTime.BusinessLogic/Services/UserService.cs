using ShowTime.BusinessLogic.Abstraction;
using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Services;

public class UserService : IUserService
{
    public readonly IGenericRepository<Artist> _userRepository;

    public UserService(IGenericRepository<Artist> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        return new LoginResponseDto
        {
            Role = DataAccess.Models.Enums.Role.User,
        };
    }
  
}