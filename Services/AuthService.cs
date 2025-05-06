using Bookstore.DTOs;
using Bookstore.Models;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services.Interfaces;
using MapsterMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity.Data;
using static Bookstore.Utils.PasswordUtils;

namespace Bookstore.Services
{
    public class AuthService : IAuthService
    {
        private IAuthRepository _authRepository;
        private IMapper _mapper;
        public AuthService(IAuthRepository authRepository, IMapper mapper)
        {
            _authRepository = authRepository;
            _mapper = mapper;
        }
        //public async Task<LogInDto> LoginAsync(LogInDto loginRequest)
        //{
        //    var user = await _authRepository.GetByEmailAsync(loginRequest.Email);
        //    if (user == null || !VerifyPasswordHash(loginRequest.Password, user.PasswordHash))
        //        return null;
        //    await _authRepository.SaveChangesAsync();
        //    var userLogIn = _mapper.Map<LogInDto>(user);
        //    return userLogIn;
        //}
        public async Task<AuthResultDto> LoginAsync(LogInDto loginRequest)
        {
            var user = await _authRepository.GetByEmailAsync(loginRequest.Email);
            if (user == null || !VerifyPasswordHash(loginRequest.Password, user.PasswordHash))
            {
                return new AuthResultDto
                {
                    Success = false,
                    ErrorMessage = "Invalid email or password."
                };
            }

            return new AuthResultDto
            {
                Success = true,
                UserId = user.Id.ToString(), // sau direct user.Id dacă e string
                UserName = $"{user.FirstName} {user.LastName}",
                UserRole = user.UserRole?.Type
            };
        }
        public async Task<AuthResultDto> RegisterAsync(RegisterDto registerRequest)
        {
            var userExists = await _authRepository.EmailExistsAsync(registerRequest.Email);
            if (userExists)
                return new AuthResultDto { Success = false, ErrorMessage = "Email already registered" };

            if (registerRequest.Password != registerRequest.ConfirmPassword)
                return new AuthResultDto { Success = false, ErrorMessage = "Passwords do not match" };

            CreatePasswordHash(registerRequest.Password, out var passwordHash);


            var userRole = await _authRepository.GetRoleByTypeAsync("user");
            if (userRole == null)
                throw new Exception("Default role 'user' not found in UserRoles table.");
            var user = new User
            {
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                PasswordHash = passwordHash,
                UserRoleId = userRole.Id
            };
            var createdUser = await _authRepository.AddAsync(user);
            await _authRepository.SaveChangesAsync();
            return new AuthResultDto { Success = true, User = createdUser };
        }
        public async Task<bool> ChangePasswordAsync(string email, string password)
        {
            var user = await _authRepository.GetByEmailAsync(email);
            if (user == null) 
                return false;
            CreatePasswordHash(password, out var passwordHash);
            user.PasswordHash = passwordHash;
            await _authRepository.SaveChangesAsync();
            return true;
        }

        public async Task<SendResetCodeResultDto?> GenerateOneTimeCodeAsync(string email)
        {
            var user = await _authRepository.GetByEmailAsync(email);

            if (user == null) return null;

            var generator = new Random();
            var code = generator.Next(0, 10000).ToString("D4");

            user.CodeResetPassword = code;
            user.TimeCodeExpires = DateTime.UtcNow.AddMinutes(5);
            await _authRepository.SaveChangesAsync();

            return _mapper.Map<SendResetCodeResultDto>(user);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _authRepository.GetByEmailAsync(email);
        }

    }
}
