using Bookstore.Models;
using System.Linq.Expressions;

namespace Bookstore.Repositories.Interfaces
{
    public interface IGenreRepository
    {
        //IGenreRepository GenreRepository { get; }
        IQueryable<Genre> FindAll();
        IQueryable<Genre> FindByCondition(Expression<Func<Genre, bool>> expression);
        void Create (Genre genre);
        void Update (Genre genre);
        void Delete (Genre genre);
        void Save();
    }
}
