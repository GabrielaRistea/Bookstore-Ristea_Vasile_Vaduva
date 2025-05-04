using Bookstore.Models;

namespace Bookstore.Services.Interfaces
{
    public interface IGenreService
    {
        IEnumerable<Genre> GetAllGenres();
        Genre? GetGenreById(int id);
        void AddGenre(Genre genre);
        void DeleteGenre(int id);
        void UpdateGenre(Genre genre);

    }
}
