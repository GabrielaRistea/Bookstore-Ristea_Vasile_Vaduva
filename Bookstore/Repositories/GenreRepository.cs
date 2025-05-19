using Bookstore.Context;
using Bookstore.Models;
using Bookstore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bookstore.Repositories
{
    public class GenreRepository : IGenreRepository
    {

        private readonly BookstoreContext _context;
        public GenreRepository(BookstoreContext context)
        {
            _context = context;
        }
        public IQueryable<Genre> FindAll() =>
            _context.Set<Genre>().AsNoTracking();

        public IQueryable<Genre> FindByCondition(Expression<Func<Genre, bool>> expression) =>
            _context.Set<Genre>().Where(expression).AsNoTracking();
        public void Create(Genre genre) =>
            _context.Set<Genre>().Add(genre);

        public void Update(Genre genre) =>
            _context.Set<Genre>().Update(genre);

        public void Delete(Genre genre) =>
            _context.Set<Genre>().Remove(genre);

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
