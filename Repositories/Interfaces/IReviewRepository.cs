using Bookstore.Models;
using System.Linq.Expressions;

namespace Bookstore.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        IQueryable<Review> FindAll();
        IQueryable<Review> FindByCondition(Expression<Func<Review, bool>> expression);
        void Create(Review review);
        void Update(Review review);
        void Delete(Review review);
        void Save();
        List<Review> GetReviewsByBookId(int bookId);
        //void AddReview(Review review);
    }
}
