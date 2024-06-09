using BookStore.Application.Controllers;
using BookStore.Application.Dtos;
using BookStore.Domain.Models;
using BookStore.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BookStore.Tests.Controllers;

public class PublisherControllerTests
{
    private readonly Mock<IPublisherService> _publisherServiceMock;
    private readonly PublisherController _publisherController;

    public PublisherControllerTests()
    {
        _publisherServiceMock = new Mock<IPublisherService>();
        _publisherController = new PublisherController(_publisherServiceMock.Object);
    }

    [Fact]
    public async Task GetAllPublishers_ReturnsOkResult_WithListOfPublishers()
    {
        // Arrange
        var publishers = new List<Publisher>
        {
            new Publisher { Id = 1, Name = "Publisher One" },
            new Publisher { Id = 2, Name = "Publisher Two" }
        };
        _publisherServiceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(publishers);

        // Act
        var result = await _publisherController.GetAllPublishers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnPublishers = Assert.IsAssignableFrom<IEnumerable<Publisher>>(okResult.Value);
        Assert.Equal(publishers.Count, returnPublishers.Count());
    }

    [Fact]
    public async Task GetPublisherById_ReturnsOkResult_WithPublisher()
    {
        // Arrange
        var publisher = new Publisher { Id = 1, Name = "Publisher One" };
        _publisherServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(publisher);

        // Act
        var result = await _publisherController.GetPublisherById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnPublisher = Assert.IsType<Publisher>(okResult.Value);
        Assert.Equal(publisher.Id, returnPublisher.Id);
    }

    [Fact]
    public async Task GetPublisherById_ReturnsNotFound_WhenPublisherNotExists()
    {
        // Arrange
        _publisherServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync((Publisher)null);

        // Act
        var result = await _publisherController.GetPublisherById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreatePublisher_ReturnsCreatedAtActionResult_WithNewPublisher()
    {
        // Arrange
        var publisherDto = new PublisherDto { Name = "Publisher One" };
        var newPublisher = new Publisher { Id = 1, Name = "Publisher One" };

        _publisherServiceMock.Setup(service => service.AddAsync(It.IsAny<Publisher>())).ReturnsAsync(newPublisher);

        // Act
        var result = await _publisherController.CreatePublisher(publisherDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnPublisher = Assert.IsType<Publisher>(createdAtActionResult.Value);
        Assert.Equal(newPublisher.Id, returnPublisher.Id);
    }

    [Fact]
    public async Task UpdatePublisher_ReturnsNoContent_WhenPublisherIsUpdated()
    {
        // Arrange
        var publisherDto = new PublisherDto { Name = "Updated Publisher" };
        var existingPublisher = new Publisher { Id = 1, Name = "Publisher One" };

        _publisherServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(existingPublisher);
        _publisherServiceMock.Setup(service => service.UpdateAsync(It.IsAny<Publisher>())).ReturnsAsync(existingPublisher);

        // Act
        var result = await _publisherController.UpdatePublisher(1, publisherDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdatePublisher_ReturnsNotFound_WhenPublisherNotExists()
    {
        // Arrange
        var publisherDto = new PublisherDto { Name = "Updated Publisher" };

        _publisherServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync((Publisher)null);

        // Act
        var result = await _publisherController.UpdatePublisher(1, publisherDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeletePublisher_ReturnsNoContent_WhenPublisherIsDeleted()
    {
        // Arrange
        _publisherServiceMock.Setup(service => service.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _publisherController.DeletePublisher(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeletePublisher_ReturnsNotFound_WhenPublisherNotExists()
    {
        // Arrange
        _publisherServiceMock.Setup(service => service.DeleteAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _publisherController.DeletePublisher(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
