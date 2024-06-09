using BookStore.Domain.Models;
using BookStore.Domain.Services;
using BookStore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Services;

public class AuthorService(ApplicationDbContext _context) : IAuthorService
{
    public async Task<Author> AddAsync(Author newAuthor)
    {
        _context.Authors.Add(newAuthor);
        await _context.SaveChangesAsync();
        return newAuthor;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Author? authorToDelete = await _context.Authors.FindAsync(id);

        if (authorToDelete is null)
        {
            return false;
        }

        _context.Authors.Remove(authorToDelete);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Author>> GetAllAsync()
    {
        return await _context.Authors.Include(a => a.Books).ToListAsync();
    }

    public async Task<Author?> GetByIdAsync(int id)
    {
        return await _context.Authors.FindAsync(id);
    }

    public async Task<Author> UpdateAsync(Author newAuthor)
    {
        Author? authorToUpdate = await _context.Authors.FindAsync(newAuthor.Id)
            ?? throw new InvalidOperationException("Author not found");

        authorToUpdate.FirstName = newAuthor.FirstName;
        authorToUpdate.LastName = newAuthor.LastName;
        _context.Authors.Update(authorToUpdate);
        await _context.SaveChangesAsync();
        return authorToUpdate;
    }
}
