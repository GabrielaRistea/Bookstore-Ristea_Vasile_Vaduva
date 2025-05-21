using Bookstore.DTOs;
using Bookstore.Models;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services;
using MapsterMapper;
using Moq;
using NUnit.Framework.Legacy;

namespace Bookstore.Tests.AuthServiceTests
{
    internal class AuthService_RegisterAsync_Tests
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
        public async Task When_EmailExists_Then_UserCannotRegister()
        {
            //arrange
            authRepository.Setup(x => x.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            var registerDTo = new RegisterDto();

            //act
            var result = await sut.RegisterAsync(registerDTo);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Email already registered"));
            authRepository.Verify(x => x.EmailExistsAsync(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task When_PasswordsDoesnotMatch_Then_UserCannotRegister()
        {
            //arrange
            authRepository.Setup(x => x.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            var registerDTo = new RegisterDto()
            {
                Password = "1",
                ConfirmPassword = "2"
            };

            //act
            var result = await sut.RegisterAsync(registerDTo);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Passwords do not match"));
        }

        [Test]
        public async Task When_RoleDoesNotExist_Then_UserCannotRegister()
        {
            //arrange
            authRepository.Setup(x => x.GetRoleByTypeAsync(It.IsAny<string>())).ReturnsAsync(null as UserRole);
            authRepository.Setup(x => x.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            var registerDTo = new RegisterDto()
            {
                Password = "1",
                ConfirmPassword = "1"
            };

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await sut.RegisterAsync(registerDTo));
        }

        [Test]
        public async Task When_UserCredentialsAreCorrect_Then_UserCanRegister()
        {
            //arrange
            authRepository.Setup(x => x.GetRoleByTypeAsync(It.IsAny<string>())).ReturnsAsync(new UserRole());
            authRepository.Setup(x => x.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            var registerDTo = new RegisterDto()
            {
                Password = "1",
                ConfirmPassword = "1"
            };

            //act
            var result = await sut.RegisterAsync(registerDTo);

            // Assert
            Assert.That(result.Success, Is.True);
            authRepository.Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
            authRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
