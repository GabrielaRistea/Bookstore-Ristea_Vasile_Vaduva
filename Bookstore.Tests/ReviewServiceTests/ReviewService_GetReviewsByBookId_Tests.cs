using Bookstore.Models;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Tests.ReviewServiceTests
{
    internal class ReviewService_GetReviewsByBookId_Tests
    {
        private Mock<IReviewRepository> _mockRepository;
        private ReviewService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IReviewRepository>();
            _service = new ReviewService(_mockRepository.Object);
        }

        [Test]
        public void When_ValidBookIdIsProvided_Then_ReturnsListOfReviews()
        {
            // Arrange
            int bookId = 123;
            var expectedReviews = new List<Review>
            {
                new Review { Id = 1, IdBook = bookId, User = new User() },
                new Review { Id = 2, IdBook = bookId, User = new User() }
            }.AsQueryable();

            _mockRepository.Setup(r =>
                r.FindByCondition(It.IsAny<Expression<Func<Review, bool>>>()))
                .Returns(expectedReviews);

            // Act
            var result = _service.GetReviewsByBookId(bookId);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.All(r => r.IdBook == bookId), Is.True);
        }

        [Test]
        public void When_NoReviewsFound_Then_ReturnsEmptyList()
        {
            // Arrange
            int bookId = 999;
            var emptyReviews = new List<Review>().AsQueryable();

            _mockRepository.Setup(r =>
                r.FindByCondition(It.IsAny<Expression<Func<Review, bool>>>()))
                .Returns(emptyReviews);

            // Act
            var result = _service.GetReviewsByBookId(bookId);

            // Assert
            Assert.That(result, Is.Empty);
        }

    }
}
