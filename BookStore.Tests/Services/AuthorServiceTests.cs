using BookStore.Application.Services;
using BookStore.Domain.Models;
using BookStore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Tests.Services;
public class AuthorServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly AuthorService _authorService;

    public AuthorServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "AuthorTestDb")
            .Options;

        _context = new ApplicationDbContext(options);
        _authorService = new AuthorService(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddAuthor()
    {
        // Arrange
        Author newAuthor = new Author
        {
            FirstName = "John",
            LastName = "Doe",
            Country = Country.US
        };

        // Act
        Author addedAuthor = await _authorService.AddAsync(newAuthor);

        // Assert
        Assert.NotNull(addedAuthor);
        Assert.Equal(newAuthor.FirstName, addedAuthor.FirstName);
        Assert.Equal(newAuthor.LastName, addedAuthor.LastName);
        Assert.Equal(newAuthor.Country, addedAuthor.Country);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAuthor()
    {
        // Arrange
        Author newAuthor = new Author
        {
            FirstName = "John",
            LastName = "Doe",
            Country = Country.US
        };
        Author addedAuthor = await _authorService.AddAsync(newAuthor);

        // Act
        Author? retrievedAuthor = await _authorService.GetByIdAsync(addedAuthor.Id);

        // Assert
        Assert.NotNull(retrievedAuthor);
        Assert.Equal(addedAuthor.Id, retrievedAuthor.Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAuthors()
    {
        // Arrange
        Author newAuthor1 = new Author
        {
            FirstName = "John",
            LastName = "Doe",
            Country = Country.US
        };
        Author newAuthor2 = new Author
        {
            FirstName = "Jane",
            LastName = "Smith",
            Country = Country.CA
        };
        await _authorService.AddAsync(newAuthor1);
        await _authorService.AddAsync(newAuthor2);

        // Act
        List<Author> authors = (await _authorService.GetAllAsync()).ToList();

        // Assert
        Assert.NotEmpty(authors);
        Assert.Contains(authors, a => a.FirstName == "John");
        Assert.Contains(authors, a => a.FirstName == "Jane");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAuthor()
    {
        // Arrange
        Author newAuthor = new Author
        {
            FirstName = "John",
            LastName = "Doe",
            Country = Country.US
        };
        Author addedAuthor = await _authorService.AddAsync(newAuthor);

        addedAuthor.FirstName = "Updated";
        addedAuthor.LastName = "Author";

        // Act
        Author updatedAuthor = await _authorService.UpdateAsync(addedAuthor);

        // Assert
        Assert.NotNull(updatedAuthor);
        Assert.Equal("Updated", updatedAuthor.FirstName);
        Assert.Equal("Author", updatedAuthor.LastName);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteAuthor()
    {
        // Arrange
        Author newAuthor = new Author
        {
            FirstName = "John",
            LastName = "Doe",
            Country = Country.US
        };
        Author addedAuthor = await _authorService.AddAsync(newAuthor);

        // Act
        bool result = await _authorService.DeleteAsync(addedAuthor.Id);

        // Assert
        Assert.True(result);
        Author? deletedAuthor = await _authorService.GetByIdAsync(addedAuthor.Id);
        Assert.Null(deletedAuthor);
    }
}
