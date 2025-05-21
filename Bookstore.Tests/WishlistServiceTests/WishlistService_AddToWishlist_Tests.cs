using Bookstore.Models;
using Bookstore.Repositories;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Tests.WishlistServiceTests
{
    internal class WishlistService_AddToWishlist_Tests
    {
        private Mock<IWishlistRepository> wishlistRepository = new();
        private WishlistService sut;
        [SetUp]
        public void Setup()
        {
            //wishlistRepository.Reset();
            sut = new WishlistService(wishlistRepository.Object);
        }
        [Test]
        public void When_BookAlreadyInWishlist_Then_DoNotAddItAgain()
        {
            // Arrange
            var wishlist = new Wishlist { Id = 1, IdUser = 1 };
            wishlistRepository.Setup(r => r.GetByUserId(1)).Returns(wishlist);
            wishlistRepository.Setup(r => r.WishlistBookExists(wishlist.Id, 5)).Returns(true);

            // Act
            sut.AddToWishlist(1, 5);

            // Assert
            wishlistRepository.Verify(r => r.AddWishlistBook(It.IsAny<WishlistBook>()), Times.Never);
            wishlistRepository.Verify(r => r.Save(), Times.Never);
        }
        [Test]
        public void When_BookIsNotInWishlist_Then_AddToWishlist()
        {
            // Arrange
            var wishlist = new Wishlist { Id = 10, IdUser = 1 };
            wishlistRepository.Setup(r => r.GetByUserId(1)).Returns(wishlist);
            wishlistRepository.Setup(r => r.WishlistBookExists(wishlist.Id, 5)).Returns(false);
            wishlistRepository.Setup(r => r.GetBookById(5)).Returns(new Book());

            // Act
            sut.AddToWishlist(1, 5);

            // Assert
            wishlistRepository.Verify(r => r.AddWishlistBook(It.Is<WishlistBook>(wb => wb.IdBook == 5 && wb.IdWishlist == 10)), Times.Once);
            wishlistRepository.Verify(r => r.Save(), Times.Once);
        }


    }
}
