using Bookstore.DTOs;
using Bookstore.Models;

namespace Bookstore.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task SaveChangesAsync();
        Task<User> AddAsync(User user);
        Task UpdateUserAsync(User user);
    }
}
