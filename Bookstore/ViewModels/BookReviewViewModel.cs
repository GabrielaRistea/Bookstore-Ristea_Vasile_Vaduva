using Bookstore.DTOs;
using Bookstore.Models;

namespace Bookstore.ViewModels
{
    public class BookReviewViewModel
    {
        public IEnumerable<Review> Reviews { get; set; } = Enumerable.Empty<Review>();
        public double AverageRating { get; set; }
    }
}
