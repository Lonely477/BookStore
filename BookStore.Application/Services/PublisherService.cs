using BookStore.Domain.Models;
using BookStore.Domain.Services;
using BookStore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Services;

public class PublisherService(ApplicationDbContext _context) : IPublisherService
{
    public async Task<Publisher> AddAsync(Publisher newPublisher)
    {
        _context.Publishers.Add(newPublisher);
        await _context.SaveChangesAsync();
        return newPublisher;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Publisher? publisherToDelete = await _context.Publishers.FindAsync(id);
        if (publisherToDelete is null)
        {
            return false;
        }

        _context.Publishers.Remove(publisherToDelete);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Publisher>> GetAllAsync()
    {
        return await _context.Publishers.Include(p => p.Books).ToListAsync();
    }

    public async Task<Publisher?> GetByIdAsync(int id)
    {
        return await _context.Publishers.FindAsync(id);
    }

    public async Task<Publisher> UpdateAsync(Publisher newPublisher)
    {
        Publisher? publisherToUpdate = await _context.Publishers.FindAsync(newPublisher.Id)
            ?? throw new InvalidOperationException("Publisher not found");

        publisherToUpdate.Name = newPublisher.Name;
        _context.Publishers.Update(publisherToUpdate);
        await _context.SaveChangesAsync();
        return publisherToUpdate;
    }
}
