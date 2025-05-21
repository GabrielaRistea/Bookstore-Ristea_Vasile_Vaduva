using Bookstore.DTOs;
using Bookstore.Models;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services;
using MapsterMapper;
using Moq;

namespace Bookstore.Tests.AuthServiceTests
{
    internal class AuthService_LoginAsync_Tests
    {
        private Mock<IAuthRepository> authRepository = new();
        private Mock<IMapper> mapper = new();
        private AuthService sut;
        [SetUp]
        public void Setup()
        {
            sut = new AuthService(authRepository.Object, mapper.Object);
        }
        [Test]
        public async Task When_EmailDoesNotExist_Then_UserCannotLogIn()
        {
            //arrange
            authRepository.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(null as User);

            //act
            var result = await sut.LoginAsync(new LogInDto { Email = "user@example.com", Password = "pass" });

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Invalid email or password."));
            authRepository.Verify(x => x.GetByEmailAsync(It.IsAny<string>()), Times.Once);
        }
        [Test]
        public async Task When_PasswordIsWrong_Then_UserCannotLogIn()
        {
            //arrange
            var user = new User
            {
                Email = "user@example.com",
                PasswordHash = "Y3eQpZ5WD8mpKVv14puB+1myAvCes0CY3xJPFySG0IDzXcgmR8cXnFAgnpjxTsy77mqgzYlUWe7iD+2HN7kzoQ==",
                FirstName = "John",
                LastName = "Doe"
            };
            authRepository.Setup(x => x.GetByEmailAsync(user.Email))
                  .ReturnsAsync(user);
            //act
            var result = await sut.LoginAsync(new LogInDto { Email = user.Email, Password = "wrongpassword" });

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Invalid email or password."));
        }
        [Test]
        public async Task When_PasswordAndEmailAreCorrect_Then_UserCanLogIn()
        {
            //arrange
            var user = new User
            {
                Email = "user@example.com",
                PasswordHash = "Y3eQpZ5WD8mpKVv14puB+1myAvCes0CY3xJPFySG0IDzXcgmR8cXnFAgnpjxTsy77mqgzYlUWe7iD+2HN7kzoQ==",
                FirstName = "John",
                LastName = "Doe"
            };
            authRepository.Setup(x => x.GetByEmailAsync(user.Email))
                  .ReturnsAsync(user);

            //act
            var result = await sut.LoginAsync(new LogInDto { Email = user.Email, Password = "parola1" });

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.User, Is.EqualTo(user));
        }
        [Test]
        public async Task When_RoleDoesNotExist_Then_UserCannotLogIn()
        {
            //arrange
            var user = new User
            {
                Email = "user@example.com",
                PasswordHash = "Y3eQpZ5WD8mpKVv14puB+1myAvCes0CY3xJPFySG0IDzXcgmR8cXnFAgnpjxTsy77mqgzYlUWe7iD+2HN7kzoQ==",
                FirstName = "John",
                LastName = "Doe",
                UserRole = null
            };
            authRepository.Setup(x => x.GetByEmailAsync(user.Email))
                  .ReturnsAsync(user);
            //act
            var result = await sut.LoginAsync(new LogInDto { Email = user.Email, Password = "wrongpassword" });

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.UserRole, Is.Null);
        }
    }
}
