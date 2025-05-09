﻿using Bookstore.Models;
using Bookstore.DTOs;

namespace Bookstore.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> LoginAsync(LogInDto loginRequest);
        Task<AuthResultDto> RegisterAsync(RegisterDto registerRequest);
        Task<bool> ChangePasswordAsync(string email, string password);
        Task<SendResetCodeResultDto?> GenerateOneTimeCodeAsync(string email);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int userId);
        Task UpdateUserAsync(User user);
    }
}
