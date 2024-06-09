using BookStore.Application.Controllers;
using BookStore.Application.Dtos;
using BookStore.Domain.Models;
using BookStore.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace BookStore.Tests.Controllers;

public class AuthorControllerTests
{
    private readonly Mock<IAuthorService> _authorServiceMock;
    private readonly AuthorController _authorController;

    public AuthorControllerTests()
    {
        _authorServiceMock = new Mock<IAuthorService>();
        _authorController = new AuthorController(_authorServiceMock.Object);
    }

    [Fact]
    public async Task GetAllAuthors_ReturnsOkResult_WithListOfAuthors()
    {
        // Arrange
        var authors = new List<Author>
        {
            new Author { Id = 1, FirstName = "John", LastName = "Doe", Country = Country.US },
            new Author { Id = 2, FirstName = "Jane", LastName = "Smith", Country = Country.GB }
        };
        _authorServiceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(authors);

        // Act
        var result = await _authorController.GetAllAuthors();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnAuthors = Assert.IsAssignableFrom<IEnumerable<Author>>(okResult.Value);
        Assert.Equal(authors.Count, returnAuthors.Count());
    }

    [Fact]
    public async Task GetAuthorById_ReturnsOkResult_WithAuthor()
    {
        // Arrange
        var author = new Author { Id = 1, FirstName = "John", LastName = "Doe", Country = Country.US };
        _authorServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(author);

        // Act
        var result = await _authorController.GetAuthorById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnAuthor = Assert.IsType<Author>(okResult.Value);
        Assert.Equal(author.Id, returnAuthor.Id);
    }

    [Fact]
    public async Task GetAuthorById_ReturnsNotFound_WhenAuthorNotExists()
    {
        // Arrange
        _authorServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync((Author)null);

        // Act
        var result = await _authorController.GetAuthorById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateAuthor_ReturnsCreatedAtActionResult_WithNewAuthor()
    {
        // Arrange
        var authorDto = new AuthorDto { FirstName = "John", LastName = "Doe", Country = Country.US };
        var newAuthor = new Author { Id = 1, FirstName = "John", LastName = "Doe", Country = Country.US };

        _authorServiceMock.Setup(service => service.AddAsync(It.IsAny<Author>())).ReturnsAsync(newAuthor);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Role, "Admin")
        }, "mock"));
        _authorController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };

        // Act
        var result = await _authorController.CreateAuthor(authorDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnAuthor = Assert.IsType<Author>(createdAtActionResult.Value);
        Assert.Equal(StatusCodes.Status201Created, createdAtActionResult.StatusCode);
        Assert.Equal(newAuthor.Id, returnAuthor.Id); // Fix the assertion

    }

    [Fact]
    public async Task UpdateAuthor_ReturnsNoContent_WhenAuthorIsUpdated()
    {
        // Arrange
        var authorDto = new AuthorDto { FirstName = "John", LastName = "Doe", Country = Country.US };
        var existingAuthor = new Author { Id = 1, FirstName = "John", LastName = "Doe", Country = Country.US };

        _authorServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(existingAuthor);
        _authorServiceMock.Setup(service => service.UpdateAsync(It.IsAny<Author>())).ReturnsAsync(existingAuthor);

        // Act
        var result = await _authorController.UpdateAuthor(1, authorDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateAuthor_ReturnsNotFound_WhenAuthorNotExists()
    {
        // Arrange
        var authorDto = new AuthorDto { FirstName = "John", LastName = "Doe", Country = Country.US };

        _authorServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync((Author)null);

        // Act
        var result = await _authorController.UpdateAuthor(1, authorDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteAuthor_ReturnsNoContent_WhenAuthorIsDeleted()
    {
        // Arrange
        _authorServiceMock.Setup(service => service.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _authorController.DeleteAuthor(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteAuthor_ReturnsNotFound_WhenAuthorNotExists()
    {
        // Arrange
        _authorServiceMock.Setup(service => service.DeleteAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _authorController.DeleteAuthor(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
