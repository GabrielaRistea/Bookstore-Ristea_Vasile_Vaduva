using Bookstore.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Context
{
    public class BookstoreContext : DbContext
    {
        public BookstoreContext(DbContextOptions<BookstoreContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorBook> AuthorBooks { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<GenreBook> GenreBooks { get; set; }
        public DbSet<HistoryOrders> HistoryOrders { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistBook> WishlistBooks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
