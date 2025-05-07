using Bookstore.Context;
using Bookstore.Models;
using Bookstore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Bookstore.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BookstoreContext _context;
        public OrderRepository(BookstoreContext context)
        {
            _context = context;
        }
        public IQueryable<Order> FindAll() =>
            _context.Set<Order>().AsNoTracking();
        public IQueryable<Order> FindByCondition(Expression<Func<Order, bool>> expression) =>
            _context.Set<Order>().Where(expression).AsNoTracking();
        public void Create(Order order) =>
            _context.Set<Order>().Add(order);
        public void Update(Order order) =>
            _context.Set<Order>().Update(order);
        public void Delete(Order order) =>
            _context.Set<Order>().Remove(order);
        public void Save()
        {
            _context.SaveChanges();
        }

        public string? GetUserEmailById(int userId)
        {
            return _context.Users
                           .Where(u => u.Id == userId)
                           .Select(u => u.Email)
                           .FirstOrDefault();
        }

    }
}
