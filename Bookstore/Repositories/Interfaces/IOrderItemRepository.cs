using Bookstore.Models;
using System.Linq.Expressions;

namespace Bookstore.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        IQueryable<OrderItem> FindAll();
        IQueryable<OrderItem> FindByCondition(Expression<Func<OrderItem, bool>> expression);
        void Add(OrderItem item);
        void Remove(OrderItem item);
        void Update(OrderItem item);
        void Save();
    }
}
