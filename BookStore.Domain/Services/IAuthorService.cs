using BookStore.Domain.Models;

namespace BookStore.Domain.Services;

public interface IAuthorService
{
    public Task<Author?> GetByIdAsync(int id);
    public Task<List<Author>> GetAllAsync();
    public Task<Author> AddAsync(Author newAuthor);
    public Task<Author> UpdateAsync(Author newAuthor);
    public Task<bool> DeleteAsync(int id);
}
