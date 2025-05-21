using Bookstore.Models;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Tests.ReviewServiceTests
{
    internal class ReviewServiceTests
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
        public void When_ValidReviewIsProvided_Then_ReviewIsSaved()
        {
            // Arrange
            var review = new Review
            {
                Rating = 5,
                Comm = "Great book!",
                IdBook = 1,
                IdUser = 2
            };

            // Act
            _service.AddReview(review);

            // Assert
            _mockRepository.Verify(r => r.Create(It.Is<Review>(rev =>
                rev.Rating == review.Rating &&
                rev.Comm == review.Comm &&
                rev.IdBook == review.IdBook &&
                rev.IdUser == review.IdUser
            )), Times.Once);

            _mockRepository.Verify(r => r.Save(), Times.Once);
        }
    }
}
