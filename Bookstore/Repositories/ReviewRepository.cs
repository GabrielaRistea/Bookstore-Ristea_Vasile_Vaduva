using Bookstore.Context;
using Bookstore.Models;
using Bookstore.Repositories.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bookstore.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly BookstoreContext _context;
        public ReviewRepository(BookstoreContext context)
        {
            _context = context;
        }
        public IQueryable<Review> FindAll() =>
            _context.Set<Review>().AsNoTracking();
        public IQueryable<Review> FindByCondition(Expression<Func<Review, bool>> expression) =>
            _context.Set<Review>().Where(expression).AsNoTracking();
        public void Create(Review review) =>
               _context.Set<Review>().Add(review);
        public void Update(Review review) =>
            _context.Set<Review>().Update(review);
        public void Delete(Review review) =>
            _context.Set<Review>().Remove(review);
        public void Save()
        {
            _context.SaveChanges();
        }

        public List<Review> GetReviewsByBookId(int bookId)
        {
            return _context.Reviews
                .Include(r => r.User)
                .Where(r => r.IdBook == bookId)
                .ToList();
        }
    }
}
