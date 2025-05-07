using Bookstore.Context;
using Bookstore.Models;
using Bookstore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bookstore.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly BookstoreContext _context;
        public OrderItemRepository(BookstoreContext context)
        {
            _context = context;
        }
        public IQueryable<OrderItem> FindAll() =>
            _context.Set<OrderItem>().AsNoTracking();
        public IQueryable<OrderItem> FindByCondition(Expression<Func<OrderItem, bool>> expression) =>
            _context.Set<OrderItem>().Where(expression).AsNoTracking();
        public void Add(OrderItem item) =>
            _context.Set<OrderItem>().Add(item);
        public void Update(OrderItem item) =>
            _context.Set<OrderItem>().Update(item);
        public void Remove(OrderItem item) =>
            _context.Set<OrderItem>().Remove(item);
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
