using Bookstore.Models;
using Bookstore.DTOs;

namespace Bookstore.Services.Interfaces
{
    public interface IReviewService
    {
        IEnumerable<Review> GetAllReviews();
        Review? GetReviewById(int id);
        void AddReview(Review review);
        void DeleteReview(int id);
        void UpdateReview(Review review);

        IEnumerable<Review> GetReviewsByBookId(int bookId);

        //void CreateReview(ReviewDto reviewDto);
    }
}
