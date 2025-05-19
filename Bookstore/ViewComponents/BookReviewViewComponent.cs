using Bookstore.Services.Interfaces;
using Bookstore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.ViewComponents
{
    public class BookReviewViewComponent : ViewComponent
    {
        private readonly IReviewService _reviewService;

        public BookReviewViewComponent(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int bookId)
        {
            var reviews = _reviewService.GetReviewsByBookId(bookId);
            var averageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;

            var viewModel = new BookReviewViewModel
            {
                Reviews = reviews,
                AverageRating = averageRating
            };

            return View(viewModel);
        }

    }
}
