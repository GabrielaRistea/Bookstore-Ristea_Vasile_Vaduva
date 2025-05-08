using Bookstore.Models;
using Bookstore.Repositories;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace Bookstore.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IOrderItemRepository _orderItemRepo;
        private readonly IBookRepository _bookRepo;

        public OrderService(IOrderRepository orderRepo, IOrderItemRepository orderItemRepo, IBookRepository bookRepo)
        {
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _bookRepo = bookRepo;
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

        public Order GetCartWithItems(int userId)
        {
            return _orderRepo.FindByCondition(o => o.IdUser == userId && o.statusOrder == "Unfinished")
                             .Include(o => o.OrderItems)
                             .ThenInclude(oi => oi.Book)
                             .FirstOrDefault();
        }

        public Order? GetOrderById(int orderId)
        {
            return _orderRepo.FindByCondition(o => o.Id == orderId)
                     .Include(o => o.OrderItems)
                     .ThenInclude(oi => oi.Book)
                     .FirstOrDefault();
        }

        public int MarkOrderAsFinished(Order order)
        {
            order.statusOrder = "Finished";
            order.Date = DateTime.Now.ToUniversalTime();
            _orderRepo.Update(order);

            _orderRepo.Save();
            return order.Id; // returnăm ID-ul
        }

        public string? GetUserEmailById(int userId)
        {
            return _orderRepo.GetUserEmailById(userId);
        }

        public void DecreaseStockForOrder(Order order)
        {
            foreach (var item in order.OrderItems)
            {
                var book = item.Book;
                if (book != null && book.Stock >= item.Quantity)
                {
                    book.Stock -= item.Quantity;
                    _bookRepo.Update(book);
                }
                else
                {
                   
                }
            }
            _bookRepo.Save();
        }

        public List<string> ValidateStock(Order order)
        {
            var errors = new List<string>();

            foreach (var item in order.OrderItems)
            {
                var book = item.Book;
                if (book == null || book.Stock < item.Quantity)
                {
                    errors.Add($"Insufficient stock for „{book?.Title ?? "unknown book"}” (available: {book?.Stock ?? 0})");
                }
            }

            return errors;
        }


    }

}
