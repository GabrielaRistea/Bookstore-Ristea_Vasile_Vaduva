using NUnit.Framework;
using Moq;
using Bookstore.Models;
using Bookstore.Services;
using Bookstore.Repositories;
using Bookstore.Repositories.Interfaces;
using System.Linq.Expressions;

namespace Bookstore.Tests.GenreServiceTests;

internal class GenreService_AddGenre_Tests
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
    public void When_ValidGenreIsProvided_Then_GenreIsSaved()
    {
        // Arrange
        var genre = new Genre();

        // Act
        _service.AddGenre(genre);

        // Assert
        _mockRepository.Verify(r => r.Create(It.IsAny<Genre>()), Times.Once);
        _mockRepository.Verify(r => r.Save(), Times.Once);
    }
}
