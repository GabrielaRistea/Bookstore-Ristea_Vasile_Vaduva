using Bookstore.Context;
using Bookstore.Models;
using Bookstore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bookstore.Repositories
{
    public class HistoryRepository : IHistoryRepository
    {
        private readonly BookstoreContext _context;
        public HistoryRepository(BookstoreContext context)
        {
            _context = context;
        }
        public IQueryable<HistoryOrders> FindAll() =>
            _context.Set<HistoryOrders>().AsNoTracking();
        public IQueryable<HistoryOrders> FindByCondition(Expression<Func<HistoryOrders, bool>> expression) =>
            _context.Set<HistoryOrders>().Where(expression).AsNoTracking();
        public void Create(HistoryOrders historyOrders) =>
               _context.Set<HistoryOrders>().Add(historyOrders);
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
