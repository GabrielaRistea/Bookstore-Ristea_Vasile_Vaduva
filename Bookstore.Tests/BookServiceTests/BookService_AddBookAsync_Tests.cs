using Bookstore.Models;
using Bookstore.Repositories;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Tests.BookServiceTests
{
    internal class BookService_AddBookAsync_Tests
    {
        private Mock<IBookRepository> bookRepository = new();
        private BookService sut;
        [SetUp]
        public void Setup()
        {
            bookRepository.Reset();
            sut = new BookService(bookRepository.Object);
        }
        [Test]
        public async Task When_ImageFileIsProvided_Then_BookImageIsSet()
        {
            // Arrange
            var imageBytes = new byte[] { 1, 2, 3, 4 };
            var imageStream = new MemoryStream(imageBytes);
            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(f => f.Length).Returns(imageBytes.Length);
            formFileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default))
                        .Returns<Stream, System.Threading.CancellationToken>((stream, _) =>
                        {
                            return imageStream.CopyToAsync(stream);
                        });

            var book = new Book { ImageFile = formFileMock.Object };

            // Act
            await sut.AddBookAsync(book);

            // Assert
            Assert.That(book.BookImage, Is.Not.Null.And.Not.Empty);
            bookRepository.Verify(r => r.Create(book), Times.Once);
            bookRepository.Verify(r => r.Save(), Times.Once);
        }
        [Test]
        public async Task When_ImageFileIsNull_Then_BookImageIsNull()
        {
            // Arrange
            var book = new Book { ImageFile = null };

            // Act
            await sut.AddBookAsync(book);

            // Assert
            Assert.That(book.BookImage, Is.Null);
            bookRepository.Verify(r => r.Create(book), Times.Once);
            bookRepository.Verify(r => r.Save(), Times.Once);
        }
        [Test]
        public async Task When_ImageFileLengthIsZero_Then_BookImageIsNotSet()
        {
            // Arrange
            var imageMock = new Mock<IFormFile>();
            imageMock.Setup(f => f.Length).Returns(0); 

            var book = new Book { ImageFile = imageMock.Object };

            // Act
            await sut.AddBookAsync(book);

            // Assert
            Assert.That(book.BookImage, Is.Null);
            bookRepository.Verify(r => r.Create(book), Times.Once);
            bookRepository.Verify(r => r.Save(), Times.Once);
        }

    }
}
