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

namespace Bookstore.Tests.GenreServiceTests
{
    internal class GenreService_DeleteGenre_Tests
    {
        private Mock<IGenreRepository> _mockRepository;
        private GenreService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IGenreRepository>();
            _service = new GenreService(_mockRepository.Object);
        }

        [Test]
        public void When_GenreExists_Then_GenreIsDeletedAndSaved()
        {
            // Arrange
            var genreId = 1;
            var genre = new Genre { Id = genreId };

            _mockRepository
             .Setup(r => r.FindByCondition(It.IsAny<Expression<Func<Genre, bool>>>()))
             .Returns(new List<Genre> { genre }.AsQueryable());

            // Act
            _service.DeleteGenre(genreId);

            // Assert
            _mockRepository.Verify(r => r.Delete(It.Is<Genre>(g => g == genre)), Times.Once);
            _mockRepository.Verify(r => r.Save(), Times.Once);
        }
    }
}
