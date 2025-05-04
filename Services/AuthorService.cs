using Bookstore.Services.Interfaces;
using Bookstore.Services;
using Bookstore.Models;
using Bookstore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Bookstore.DTOs;

namespace Bookstore.Services
{
    public class AuthorService : IAuthorService
    {
        private IAuthorRepository _authorRepository;
        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public List<Author> GetAllAuthors()
        {
            return _authorRepository.GetAll().ToList();
        }
        public async Task AddAuthorAsync(Author author)
        {
            if (author.ImageFile != null && author.ImageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await author.ImageFile.CopyToAsync(ms);
                    author.AuthorImage = ms.ToArray();
                }
            }
            _authorRepository.Create(author);
            _authorRepository.Save();
        }
        public async Task UpdateAuthorAsync(Author author)
        {
            using var ms = new MemoryStream();

            if (author.ImageFile != null && author.ImageFile.Length > 0)
            {
                await author.ImageFile.CopyToAsync(ms);
                author.AuthorImage = ms.ToArray();
            }
            _authorRepository.Update(author);
            _authorRepository.Save();
        }
        public void DeleteAuthor(int id)
        {
            var author = _authorRepository.GetById(id);
            if (author != null)
            {
                _authorRepository.Delete(author);
                _authorRepository.Save();
            }
        }
        public bool AuthorExists(int id)
        {
            return _authorRepository.AuthorExists(id);
        }
        public Author GetAuthorById(int id)
        {
            return _authorRepository.GetById(id);
        }
        public List<AuthorBook> GetAllAuthorBooks()
        {
            return _authorRepository.GetAllAuthorBooks().ToList();
        }
        public AuthorDto GetAllAuthorDetails(int authorId)
        {
            var author = _authorRepository.GetById(authorId);
            var result = new AuthorDto
            {
                Name = author.Name,
                AuthorImage = author.AuthorImage,
                Description = author.Description,
                Id = author.Id,
            };
            return result;
        }
    }
}
