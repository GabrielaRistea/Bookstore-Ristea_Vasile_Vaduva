using Bookstore.Repositories.Interfaces;
using Bookstore.Context;
using Bookstore.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private BookstoreContext _context;
        public AuthorRepository(BookstoreContext bookstoreContext)
        {
            _context = bookstoreContext;
        }

        public IEnumerable<Author> GetAll()
        {
            return _context.Authors
                .Include(a => a.AuthorBooks);
        }
        public void Create(Author author)
        {
            _context.Authors.Add(author);
        }
        public void Delete(Author author)
        {
            _context.Authors.Remove(author);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public bool AuthorExists(int id)
        {
            return _context.Authors.Any(a => a.Id == id);
        }
        public void Update(Author author)
        {
            _context.Authors.Update(author);
        }
        public Author GetById(int id)
        {
            return _context.Authors.FirstOrDefault(a => a.Id == id);
        }
        public List<AuthorBook> GetAllAuthorBooks()
        {
            return _context.AuthorBooks.ToList();
        }
        public Author GetAuthorWithBooks(int authorId)
        {
            return _context.Authors
                .Include(a => a.AuthorBooks)
                    .ThenInclude(ab => ab.Book)
                .FirstOrDefault(a => a.Id == authorId);
        }
    }
}
