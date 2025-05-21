using Bookstore.Models;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Tests.GenreServiceTests
{
    internal class GenreService_UpdateGenre_Tests
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
        public void When_ValidGenreIsUpdated_Then_GenreIsUpdatedAndSaved()
        {
            // Arrange
            var genre = new Genre();

            // Act
            _service.UpdateGenre(genre);

            // Assert
            _mockRepository.Verify(r => r.Update(It.Is<Genre>(g => g == genre)), Times.Once);
            _mockRepository.Verify(r => r.Save(), Times.Once);
        }
    }
}
