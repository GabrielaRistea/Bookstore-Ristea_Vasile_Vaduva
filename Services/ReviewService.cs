using Bookstore.DTOs;
using Bookstore.Models;
using Bookstore.Repositories;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repository;
        public ReviewService(IReviewRepository repository)
        {
            _repository = repository;
        }
        public IEnumerable<Review> GetAllReviews()
        {
            return _repository.FindAll().ToList();
        }
        public Review? GetReviewById(int id)
        {
            return _repository.FindByCondition(r => r.Id == id).FirstOrDefault();
        }
        public void AddReview(Review review)
        {
            _repository.Create(review);
            _repository.Save();
        }
        public void UpdateReview(Review review)
        {
            _repository.Update(review);
            _repository.Save();
        }
        public void DeleteReview(int id)
        {
            var review = GetReviewById(id);
            if (review != null)
            {
                _repository.Delete(review);
                _repository.Save();
            }
        }

        public IEnumerable<Review> GetReviewsByBookId(int bookId)
        {
            return _repository.FindByCondition(r => r.IdBook == bookId)
                      .Include(r => r.User)
                      .ToList();
        }

        //public void CreateReview(ReviewDto reviewDto)
        //{
        //    var review = new Review
        //    {
        //        Comm = reviewDto.Comm,
        //        Rating = reviewDto.Rating,
        //        IdBook = reviewDto.IdBook,
        //        IdUser = reviewDto.IdUser
        //    };

        //    _repository.AddReview(review);
        //}
    }
}
