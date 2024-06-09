using BookStore.Domain.Models;

namespace BookStore.Domain.Services;

public interface IPublisherService
{
    public Task<Publisher?> GetByIdAsync(int id);
    public Task<List<Publisher>> GetAllAsync();
    public Task<Publisher> AddAsync(Publisher newPublisher);
    public Task<Publisher> UpdateAsync(Publisher newPublisher);
    public Task<bool> DeleteAsync(int id);
}
