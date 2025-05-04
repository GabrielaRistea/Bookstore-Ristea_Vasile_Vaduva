using Bookstore.Models;

namespace Bookstore.Repositories.Interfaces
{
    public interface IAuthorRepository
    {
        IEnumerable<Author> GetAll();
        bool AuthorExists(int id);
        void Create(Author author);
        void Update(Author author);
        void Delete(Author author);
        void Save();
        Author GetById(int id);
        List<AuthorBook> GetAllAuthorBooks();
    }
}
