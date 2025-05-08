using Bookstore.Models;
using System.Linq.Expressions;

namespace Bookstore.Repositories.Interfaces
{
    public interface IHistoryRepository
    {
        IQueryable<HistoryOrders> FindAll();
        IQueryable<HistoryOrders> FindByCondition(Expression<Func<HistoryOrders, bool>> expression);
        void Create(HistoryOrders historyOrders);
        void Save();
    }
}
