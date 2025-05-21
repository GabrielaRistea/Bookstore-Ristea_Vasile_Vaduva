using Bookstore.Models;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Tests.AuthorServiceTests
{
    internal class AuthorService_UpdateAuthorAsync_Tests
    {
        private Mock<IAuthorRepository> authorRepository = new();
        private Mock<IBookRepository> bookRepository = new();
        private AuthorService sut;
        [SetUp]
        public void Setup()
        {
            authorRepository.Reset();
            bookRepository.Reset();
            sut = new AuthorService(authorRepository.Object, bookRepository.Object);
        }
        [Test]
        public async Task When_ImageFileExists_Then_AuthorImageIsSet()
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

            var author = new Author
            {
                ImageFile = formFileMock.Object
            };

            // Act
            await sut.UpdateAuthorAsync(author);

            // Assert
            Assert.That(author.AuthorImage, Is.Not.Null);
            Assert.That(author.AuthorImage.Length, Is.EqualTo(imageBytes.Length));
            authorRepository.Verify(x => x.Update(author), Times.Once);
            authorRepository.Verify(x => x.Save(), Times.Once);
        }
        [Test]
        public async Task When_ImageFileIsNull_Then_AuthorImageIsNotSet()
        {
            // Arrange
            var author = new Author
            {
                ImageFile = null
            };

            // Act
            await sut.UpdateAuthorAsync(author);

            // Assert
            Assert.That(author.AuthorImage, Is.Null);
            authorRepository.Verify(x => x.Update(author), Times.Once);
            authorRepository.Verify(x => x.Save(), Times.Once);
        }
        [Test]
        public async Task When_ImageFileLengthIsZero_Then_AuthorImageIsNotSet()
        {
            // Arrange
            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(f => f.Length).Returns(0);

            var author = new Author
            {
                ImageFile = formFileMock.Object
            };

            // Act
            await sut.UpdateAuthorAsync(author);

            // Assert
            Assert.That(author.AuthorImage, Is.Null);
            authorRepository.Verify(x => x.Update(author), Times.Once);
            authorRepository.Verify(x => x.Save(), Times.Once);
        }

    }
}
