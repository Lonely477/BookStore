using BookStore.Application.Services;
using BookStore.Domain.Models;
using BookStore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Tests.Services;
public class BookServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly BookService _bookService;

    public BookServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "BookTestDb")
            .Options;

        _context = new ApplicationDbContext(options);
        _bookService = new BookService(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddBook()
    {
        // Arrange
        Author author = new Author { FirstName = "John", LastName = "Doe", Country = Country.US };
        Publisher publisher = new Publisher { Name = "Penguin Books" };
        _context.Authors.Add(author);
        _context.Publishers.Add(publisher);
        await _context.SaveChangesAsync();

        Book newBook = new Book
        {
            Title = "New Book",
            Description = "Description",
            Price = 19.99m,
            PublishDate = new DateOnly(2023, 6, 1),
            Pages = 100,
            AuthorId = author.Id,
            PublisherId = publisher.Id
        };

        // Act
        Book addedBook = await _bookService.AddAsync(newBook);

        // Assert
        Assert.NotNull(addedBook);
        Assert.Equal(newBook.Title, addedBook.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBook()
    {
        // Arrange
        Author author = new Author { FirstName = "John", LastName = "Doe", Country = Country.US };
        Publisher publisher = new Publisher { Name = "Penguin Books" };
        _context.Authors.Add(author);
        _context.Publishers.Add(publisher);
        await _context.SaveChangesAsync();

        Book newBook = new Book
        {
            Title = "New Book",
            Description = "Description",
            Price = 19.99m,
            PublishDate = new DateOnly(2023, 6, 1),
            Pages = 100,
            AuthorId = author.Id,
            PublisherId = publisher.Id
        };
        Book addedBook = await _bookService.AddAsync(newBook);

        // Act
        Book? retrievedBook = await _bookService.GetByIdAsync(addedBook.Id);

        // Assert
        Assert.NotNull(retrievedBook);
        Assert.Equal(addedBook.Id, retrievedBook.Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllBooks()
    {
        // Arrange
        Author author = new Author { FirstName = "John", LastName = "Doe", Country = Country.US };
        Publisher publisher = new Publisher { Name = "Penguin Books" };
        _context.Authors.Add(author);
        _context.Publishers.Add(publisher);
        await _context.SaveChangesAsync();

        Book newBook1 = new Book
        {
            Title = "New Book 1",
            Description = "Description 1",
            Price = 19.99m,
            PublishDate = new DateOnly(2023, 6, 1),
            Pages = 100,
            AuthorId = author.Id,
            PublisherId = publisher.Id
        };
        Book newBook2 = new Book
        {
            Title = "New Book 2",
            Description = "Description 2",
            Price = 29.99m,
            PublishDate = new DateOnly(2023, 7, 1),
            Pages = 200,
            AuthorId = author.Id,
            PublisherId = publisher.Id
        };
        await _bookService.AddAsync(newBook1);
        await _bookService.AddAsync(newBook2);

        // Act
        List<Book> books = (await _bookService.GetAllAsync()).ToList();

        // Assert
        Assert.NotEmpty(books);
        Assert.Contains(books, b => b.Title == "New Book 1");
        Assert.Contains(books, b => b.Title == "New Book 2");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateBook()
    {
        // Arrange
        Author author = new Author { FirstName = "John", LastName = "Doe", Country = Country.US };
        Publisher publisher = new Publisher { Name = "Penguin Books" };
        _context.Authors.Add(author);
        _context.Publishers.Add(publisher);
        await _context.SaveChangesAsync();

        Book newBook = new Book
        {
            Title = "New Book",
            Description = "Description",
            Price = 19.99m,
            PublishDate = new DateOnly(2023, 6, 1),
            Pages = 100,
            AuthorId = author.Id,
            PublisherId = publisher.Id
        };
        Book addedBook = await _bookService.AddAsync(newBook);

        addedBook.Title = "Updated Book";
        addedBook.Description = "Updated Description";

        // Act
        Book updatedBook = await _bookService.UpdateAsync(addedBook);

        // Assert
        Assert.NotNull(updatedBook);
        Assert.Equal("Updated Book", updatedBook.Title);
        Assert.Equal("Updated Description", updatedBook.Description);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteBook()
    {
        // Arrange
        Author author = new Author { FirstName = "John", LastName = "Doe", Country = Country.US };
        Publisher publisher = new Publisher { Name = "Penguin Books" };
        _context.Authors.Add(author);
        _context.Publishers.Add(publisher);
        await _context.SaveChangesAsync();

        Book newBook = new Book
        {
            Title = "New Book",
            Description = "Description",
            Price = 19.99m,
            PublishDate = new DateOnly(2023, 6, 1),
            Pages = 100,
            AuthorId = author.Id,
            PublisherId = publisher.Id
        };
        Book addedBook = await _bookService.AddAsync(newBook);

        // Act
        bool result = await _bookService.DeleteAsync(addedBook.Id);

        // Assert
        Assert.True(result);
        Book? deletedBook = await _bookService.GetByIdAsync(addedBook.Id);
        Assert.Null(deletedBook);
    }
}
