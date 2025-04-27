using Bookstore.Models;
using Bookstore.DTOs;

namespace Bookstore.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LogInDto> LoginAsync(LogInDto loginRequest);
        Task<AuthResultDto> RegisterAsync(RegisterDto registerRequest);
        Task<bool> ChangePasswordAsync(string email, string password);
        Task<SendResetCodeResultDto?> GenerateOneTimeCodeAsync(string email);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
