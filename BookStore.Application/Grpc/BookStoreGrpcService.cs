using BookStore.Infrastructure;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Grpc;

public class BookStoreGrpcService : BookStore.BookStoreBase
{
    private readonly ApplicationDbContext _context;

    public BookStoreGrpcService(ApplicationDbContext context)
    {
        _context = context;
    }

    public override async Task<BooksResponse> GetBooksByAuthor(AuthorRequest request, ServerCallContext context)
    {
        var books = await _context.Books
            .Where(b => b.Author.LastName == request.LastName)
            .Select(b => b.Title)
            .ToListAsync();

        BooksResponse response = new();
        if (books.Any())
        {
            response.Titles.AddRange(books);
        }
        else
        {
            response.Titles.Add("No books found");
        }

        return response;
    }
}
