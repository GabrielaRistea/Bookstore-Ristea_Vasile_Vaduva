using Bookstore.DTOs;
using Bookstore.Models;

namespace Bookstore.Services.Interfaces
{
    public interface IAuthorService
    {
        List<Author> GetAllAuthors();
        Task AddAuthorAsync(Author author);
        Task UpdateAuthorAsync(Author author);
        void DeleteAuthor(int id);
        bool AuthorExists(int id);
        Author GetAuthorById(int id);
        List<AuthorBook> GetAllAuthorBooks();
        AuthorDto GetAllAuthorDetails(int authorId);
    }
}
