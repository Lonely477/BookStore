using BookStore.Domain.Models;
using BookStore.Domain.Services;
using BookStore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Services;

public class BookService : IBookService
{
    private readonly ApplicationDbContext _context;

    public BookService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Book> AddAsync(Book newBook)
    {
        _context.Books.Add(newBook);
        await _context.SaveChangesAsync();
        return newBook;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Book? bookToDelete = await _context.Books.FindAsync(id);
        if (bookToDelete is null)
        {
            return false;
        }

        _context.Books.Remove(bookToDelete);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Book>> GetAllAsync()
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Publisher)
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Book> UpdateAsync(Book newBook)
    {
        Book? bookToUpdate = await _context.Books.FindAsync(newBook.Id)
            ?? throw new InvalidOperationException("Book not found");

        bookToUpdate.Title = newBook.Title;
        bookToUpdate.Description = newBook.Description;
        bookToUpdate.Price = newBook.Price;
        bookToUpdate.PublishDate = newBook.PublishDate;
        bookToUpdate.Pages = newBook.Pages;
        bookToUpdate.AuthorId = newBook.AuthorId;
        bookToUpdate.PublisherId = newBook.PublisherId;

        _context.Books.Update(bookToUpdate);
        await _context.SaveChangesAsync();
        return bookToUpdate;
    }
}
