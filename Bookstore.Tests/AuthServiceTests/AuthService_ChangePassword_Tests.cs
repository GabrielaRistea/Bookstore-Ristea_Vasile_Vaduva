using Bookstore.Controllers;
using Bookstore.DTOs;
using Bookstore.Models;
using Bookstore.Repositories;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services;
using Bookstore.Services.Interfaces;
using MapsterMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Bookstore.Tests.AuthServiceTests
{
    internal class AuthService_ChangePassword_Tests
    {
        private Mock<IAuthRepository> authRepository = new();
        private Mock<IMapper> mapper = new();
        private AuthService sut;

        [SetUp]
        public void Setup()
        {
            authRepository.Reset();
            mapper.Reset();
            sut = new AuthService (authRepository.Object, mapper.Object);
        }
        [Test]
        public async Task When_EmailExists_Then_UserCanChangePassword()
        {
            //arrange
            var email = "anyemail@example.com"; 
            var password = "parola1";
            var user = new User
            {
                PasswordHash = "Y3eQpZ5WD8mpKVv14puB+1myAvCes0CY3xJPFySG0IDzXcgmR8cXnFAgnpjxTsy77mqgzYlUWe7iD+2HN7kzoQ=="
            };
            authRepository
                .Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            authRepository
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);


            //act
            var result = await sut.ChangePasswordAsync(email, password);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(user.PasswordHash, Is.Not.Null.And.Not.Empty);
            authRepository.Verify(x => x.GetByEmailAsync(It.IsAny<string>()), Times.Once);
            authRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        [Test]
        public async Task When_EmailDoesNotExist_Then_UserCannotChangePassword()
        {
            // Arrange
            authRepository
                .Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await sut.ChangePasswordAsync("anyemail@example.com", "anyPassword");

            // Assert
            Assert.That(result, Is.False);
            authRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}
