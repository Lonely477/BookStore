using BookStore.Domain.Models;

namespace BookStore.Domain.Services;

public interface IBookService
{
    public Task<Book?> GetByIdAsync(int id);
    public Task<List<Book>> GetAllAsync();
    public Task<Book> AddAsync(Book newBook);
    public Task<Book> UpdateAsync(Book newBook);
    public Task<bool> DeleteAsync(int id);
}
