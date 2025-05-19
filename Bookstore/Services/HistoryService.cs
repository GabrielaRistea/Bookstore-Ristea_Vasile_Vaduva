using Bookstore.Services.Interfaces;
using Bookstore.Repositories.Interfaces;
using Bookstore.Models;
using Microsoft.EntityFrameworkCore;
using Bookstore.Repositories;

namespace Bookstore.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly IHistoryRepository _historyRepo;
        private readonly IOrderRepository _orderRepo;
        public HistoryService(IHistoryRepository historyRepo, IOrderRepository orderRepo)
        {
            _historyRepo = historyRepo;
            _orderRepo = orderRepo;
        }
        public IEnumerable<HistoryOrders> GetHistoryOrders()
        {
            return _historyRepo.FindAll()
                .Include(h => h.Orders)
                    .ThenInclude(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book)
                .AsEnumerable()
                .Where(h => h.Orders.Any(o =>
                    o.OrderItems != null &&
                    o.OrderItems.Any(oi => oi.Book != null))) 
                .ToList();
        }
        public Order? GetFinishedOrderById(int orderId)
        {
            return _orderRepo.GetFinishedOrderById(orderId);
        }

    }
}
