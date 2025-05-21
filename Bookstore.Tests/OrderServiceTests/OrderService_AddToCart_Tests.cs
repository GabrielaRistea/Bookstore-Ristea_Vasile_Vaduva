using Bookstore.Repositories.Interfaces;
using Bookstore.Services;
using Moq;
using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Bookstore.Tests.OrderServiceTests
{
    internal class OrderService_AddToCart_Tests
    {
        private Mock<IOrderRepository> _mockOrderRepo;
        private Mock<IOrderItemRepository> _mockOrderItemRepo;
        private Mock<IHistoryRepository> _mockHistoryRepo;
        private Mock<IBookRepository> _mockBookRepo;
        private OrderService _service;

        [SetUp]
        public void Setup()
        {
            _mockOrderRepo = new Mock<IOrderRepository>();
            _mockOrderItemRepo = new Mock<IOrderItemRepository>();
            _mockHistoryRepo = new Mock<IHistoryRepository>();
            _mockBookRepo = new Mock<IBookRepository>();

            _service = new OrderService(
                _mockOrderRepo.Object,
                _mockOrderItemRepo.Object,
                _mockHistoryRepo.Object,
                _mockBookRepo.Object
            );
        }

        [Test]
        public void When_ItemAlreadyExistsInCart_Then_QuantityIsIncreased()
        {
            // Arrange
            var userId = It.IsAny<int>();
            var bookId = It.IsAny<int>();
            var quantity = 2;

            var cart = new Order { Id = 1, IdUser = userId };
            var existingItem = new OrderItem { IdOrder = cart.Id, IdBook = bookId, Quantity = 1 };

            _mockOrderRepo.Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Order, bool>>>()))
                          .Returns(new[] { cart }.AsQueryable());

            _mockOrderItemRepo.Setup(r => r.FindByCondition(It.IsAny<Expression<Func<OrderItem, bool>>>()))
                              .Returns(new[] { existingItem }.AsQueryable());

            // Act
            _service.AddToCart(userId, bookId, quantity);

            // Assert
            _mockOrderItemRepo.Verify(r => r.Update(It.Is<OrderItem>(oi =>
                oi.IdOrder == cart.Id &&
                oi.IdBook == bookId &&
                oi.Quantity == 3)), Times.Once);

            _mockOrderItemRepo.Verify(r => r.Save(), Times.Once);
        }

        [Test]
        public void When_ItemDoesNotExistInCart_Then_NewItemIsAdded()
        {
            // Arrange
            var userId = It.IsAny<int>();
            var bookId = It.IsAny<int>();
            var quantity = 5;

            var cart = new Order { Id = 2, IdUser = userId };

            _mockOrderRepo.Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Order, bool>>>()))
                          .Returns(new[] { cart }.AsQueryable());

            _mockOrderItemRepo.Setup(r => r.FindByCondition(It.IsAny<Expression<Func<OrderItem, bool>>>()))
                              .Returns(Enumerable.Empty<OrderItem>().AsQueryable());

            // Act
            _service.AddToCart(userId, bookId, quantity);

            // Assert
            _mockOrderItemRepo.Verify(r => r.Add(It.Is<OrderItem>(oi =>
                oi.IdOrder == cart.Id &&
                oi.IdBook == bookId &&
                oi.Quantity == quantity)), Times.Once);

            _mockOrderItemRepo.Verify(r => r.Save(), Times.Once);
        }
    }
}
