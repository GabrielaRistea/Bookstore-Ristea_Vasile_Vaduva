using Bookstore.Models;
using System.Linq.Expressions;

namespace Bookstore.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        IQueryable<Order> FindAll();
        IQueryable<Order> FindByCondition(Expression<Func<Order, bool>> expression);
        void Create(Order order);
        void Update(Order order); 
        void Delete(Order order);
        void Save();
        string? GetUserEmailById(int userId);
    }
}
