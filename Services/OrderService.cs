using Bookstore.Models;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IOrderItemRepository _orderItemRepo;

        public OrderService(IOrderRepository orderRepo, IOrderItemRepository orderItemRepo)
        {
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
        }

        public Order GetOrCreateCart(int userId)
        {
            var cart = _orderRepo.FindByCondition(o => o.IdUser == userId && o.statusOrder == "Unfinished")
                                 .FirstOrDefault();

            if (cart == null)
            {
                cart = new Order
                {
                    IdUser = userId,
                    Date = DateTime.Now.ToUniversalTime(),
                    statusOrder = "Unfinished",
                    OrderItems = new List<OrderItem>()
                };
                _orderRepo.Create(cart);
                _orderRepo.Save();
            }

            return cart;
        }

        public void AddToCart(int userId, int bookId, int quantity)
        {
            var cart = GetOrCreateCart(userId);
            var existingItem = _orderItemRepo.FindByCondition(oi => oi.IdOrder == cart.Id && oi.IdBook == bookId)
                                             .FirstOrDefault();

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                _orderItemRepo.Update(existingItem);
            }
            else
            {
                var newItem = new OrderItem
                {
                    IdOrder = cart.Id,
                    IdBook = bookId,
                    Quantity = quantity
                };
                _orderItemRepo.Add(newItem);
            }

            _orderItemRepo.Save();
        }

        public void RemoveFromCart(int userId, int bookId)
        {
            var cart = GetOrCreateCart(userId);
            var item = _orderItemRepo.FindByCondition(oi => oi.IdOrder == cart.Id && oi.IdBook == bookId)
                                     .FirstOrDefault();
            if (item != null)
            {
                _orderItemRepo.Remove(item);
                _orderItemRepo.Save();
            }
        }

        public void ClearCart(int userId)
        {
            var cart = GetOrCreateCart(userId);
            var items = _orderItemRepo.FindByCondition(oi => oi.IdOrder == cart.Id).ToList();
            foreach (var item in items)
            {
                _orderItemRepo.Remove(item);
            }
            _orderItemRepo.Save();
        }

        public void FinalizeOrder(int userId)
        {
            var cart = GetOrCreateCart(userId);
            cart.statusOrder = "Finished";
            cart.Date = DateTime.Now;
            _orderRepo.Update(cart);
            _orderRepo.Save();
        }

        public Order GetCartWithItems(int userId)
        {
            return _orderRepo.FindByCondition(o => o.IdUser == userId && o.statusOrder == "Unfinished")
                             .Include(o => o.OrderItems)
                             .ThenInclude(oi => oi.Book)
                             .FirstOrDefault();
        }
    }

}
