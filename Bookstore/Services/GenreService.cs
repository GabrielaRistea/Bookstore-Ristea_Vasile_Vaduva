using Bookstore.Models;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services.Interfaces;

namespace Bookstore.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _repository;
        public GenreService(IGenreRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Genre> GetAllGenres()
        {
            return _repository.FindAll().ToList();
        }
        public Genre? GetGenreById(int id)
        {
            return _repository.FindByCondition(g  => g.Id == id).FirstOrDefault();
        }
        public void AddGenre(Genre genre)
        {
            _repository.Create(genre);
            _repository.Save();
        }
        public void UpdateGenre(Genre genre)
        {
            _repository.Update(genre);
            _repository.Save();
        }
        public void DeleteGenre(int id)
        {
            var genre = GetGenreById(id);
            if (genre != null)
            {
                _repository.Delete(genre);
                _repository.Save();
            }
        }
    }
}
