using Bookstore.DTOs;
using Bookstore.Repositories.Interfaces;
using Bookstore.Context;
using Mapster;
using MapsterMapper;
using Bookstore.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private BookstoreContext _context;
        private IMapper _mapper;
        public AuthRepository(BookstoreContext bookstoreContext, IMapper mapper)
        {
            _context = bookstoreContext;
            _mapper = mapper;
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<User> AddAsync(User user)
        {
            var entry = await _context.Users.AddAsync(user);
            return entry.Entity;
        }
        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<UserRole> GetRoleByTypeAsync(string roleType)
        {
            return await _context.UserRoles.FirstOrDefaultAsync(r => r.Type == roleType);
        }
        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
