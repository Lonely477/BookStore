using BookStore.Application.Services;
using BookStore.Domain.Models;
using BookStore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Tests.Services;
public class PublisherServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly PublisherService _publisherService;

    public PublisherServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "PublisherTestDb")
            .Options;

        _context = new ApplicationDbContext(options);
        _publisherService = new PublisherService(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddPublisher()
    {
        // Arrange
        Publisher newPublisher = new Publisher { Name = "Penguin Books" };

        // Act
        Publisher addedPublisher = await _publisherService.AddAsync(newPublisher);

        // Assert
        Assert.NotNull(addedPublisher);
        Assert.Equal(newPublisher.Name, addedPublisher.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnPublisher()
    {
        // Arrange
        Publisher newPublisher = new Publisher { Name = "Penguin Books" };
        Publisher addedPublisher = await _publisherService.AddAsync(newPublisher);

        // Act
        Publisher? retrievedPublisher = await _publisherService.GetByIdAsync(addedPublisher.Id);

        // Assert
        Assert.NotNull(retrievedPublisher);
        Assert.Equal(addedPublisher.Id, retrievedPublisher.Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllPublishers()
    {
        // Arrange
        Publisher newPublisher1 = new Publisher { Name = "Penguin Books" };
        Publisher newPublisher2 = new Publisher { Name = "HarperCollins" };
        await _publisherService.AddAsync(newPublisher1);
        await _publisherService.AddAsync(newPublisher2);

        // Act
        List<Publisher> publishers = (await _publisherService.GetAllAsync()).ToList();

        // Assert
        Assert.NotEmpty(publishers);
        Assert.Contains(publishers, p => p.Name == "Penguin Books");
        Assert.Contains(publishers, p => p.Name == "HarperCollins");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdatePublisher()
    {
        // Arrange
        Publisher newPublisher = new Publisher { Name = "Penguin Books" };
        Publisher addedPublisher = await _publisherService.AddAsync(newPublisher);

        addedPublisher.Name = "Updated Publisher";

        // Act
        Publisher updatedPublisher = await _publisherService.UpdateAsync(addedPublisher);

        // Assert
        Assert.NotNull(updatedPublisher);
        Assert.Equal("Updated Publisher", updatedPublisher.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeletePublisher()
    {
        // Arrange
        Publisher newPublisher = new Publisher { Name = "Penguin Books" };
        Publisher addedPublisher = await _publisherService.AddAsync(newPublisher);

        // Act
        bool result = await _publisherService.DeleteAsync(addedPublisher.Id);

        // Assert
        Assert.True(result);
        Publisher? deletedPublisher = await _publisherService.GetByIdAsync(addedPublisher.Id);
        Assert.Null(deletedPublisher);
    }
}
