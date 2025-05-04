using Bookstore.Services; // sau namespace-ul unde e IGenreService
using Bookstore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bookstore.ViewComponents
{
    public class GenreDropdownViewComponent : ViewComponent
    {
        private readonly IGenreService _genreService;

        public GenreDropdownViewComponent(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var genres = _genreService.GetAllGenres();
            return View(genres);
        }
    }
}
