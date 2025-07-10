using ShowTime.BusinessLogic.Abstraction;
using ShowTime.BusinessLogic.Dtos;
using ShowTime.DataAccess.Models;
using ShowTime.DataAccess.Models.Enums;
using ShowTime.DataAccess.Repositories.Abstraction;
using BC = BCrypt.Net.BCrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowTime.BusinessLogic.Services;

public class UserService : IUserService
{
    private readonly IGenericRepository<User> _userRepository;

    public UserService(IGenericRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email.ToLower() == loginDto.Email.ToLower());

            if (user == null || !BC.Verify(loginDto.Password, user.Password))
            {
                return null;
            }

            return new LoginResponseDto
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<string> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            if (registerDto.Password != registerDto.ConfirmPassword)
                return "Passwords do not match.";

            if (string.IsNullOrWhiteSpace(registerDto.Username))
                return "Username is required.";

            if (string.IsNullOrWhiteSpace(registerDto.Email))
                return "Email is required.";

            if (string.IsNullOrWhiteSpace(registerDto.Password))
                return "Password is required.";

            var users = await _userRepository.GetAllAsync();

            if (users.Any(u => u.Email.ToLower() == registerDto.Email.ToLower()))
                return "A user with this email already exists.";

            if (users.Any(u => u.Username.ToLower() == registerDto.Username.ToLower()))
                return "This username is already taken.";

            var newUser = new User
            {
                Username = registerDto.Username.Trim(),
                Email = registerDto.Email.Trim().ToLower(),
                Password = BC.HashPassword(registerDto.Password, 12),
                Role = Role.User
            };

            await _userRepository.AddAsync(newUser);
            return "SUCCESS";
        }
        catch (Exception ex)
        {
            return $"Database Error: {ex.Message}";
        }
    }
}