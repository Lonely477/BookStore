using BookStore.Application.Controllers;
using BookStore.Application.Dtos;
using BookStore.Domain.Models;
using BookStore.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace BookStore.Tests.Controllers;

public class BookControllerTests
{
    private readonly Mock<IBookService> _bookServiceMock;
    private readonly BookController _bookController;

    public BookControllerTests()
    {
        _bookServiceMock = new Mock<IBookService>();
        _bookController = new BookController(_bookServiceMock.Object);
    }

    [Fact]
    public async Task GetAllBooks_ReturnsOkResult_WithListOfBooks()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book { Id = 1, Title = "Book One", Description = "Description One", Price = 10.99m, PublishDate = new DateOnly(2020, 1, 1), Pages = 100, AuthorId = 1, PublisherId = 1 },
            new Book { Id = 2, Title = "Book Two", Description = "Description Two", Price = 15.99m, PublishDate = new DateOnly(2021, 1, 1), Pages = 200, AuthorId = 2, PublisherId = 2 }
        };
        _bookServiceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(books);

        // Act
        var result = await _bookController.GetAllBooks();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnBooks = Assert.IsAssignableFrom<IEnumerable<Book>>(okResult.Value);
        Assert.Equal(books.Count, returnBooks.Count());
    }

    [Fact]
    public async Task GetBookById_ReturnsOkResult_WithBook()
    {
        // Arrange
        var book = new Book { Id = 1, Title = "Book One", Description = "Description One", Price = 10.99m, PublishDate = new DateOnly(2020, 1, 1), Pages = 100, AuthorId = 1, PublisherId = 1 };
        _bookServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(book);

        // Act
        var result = await _bookController.GetBookById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnBook = Assert.IsType<Book>(okResult.Value);
        Assert.Equal(book.Id, returnBook.Id);
    }

    [Fact]
    public async Task GetBookById_ReturnsNotFound_WhenBookNotExists()
    {
        // Arrange
        _bookServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync((Book)null);

        // Act
        var result = await _bookController.GetBookById(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateBook_ReturnsCreatedAtActionResult_WithNewBook()
    {
        // Arrange
        var bookDto = new BookDto
        {
            Title = "New Book",
            Description = "New Description",
            Price = 20.99m,
            PublishDate = new DateOnly(2022, 1, 1),
            Pages = 150,
            AuthorId = 1,
            PublisherId = 1
        };
        var newBook = new Book
        {
            Id = 1,
            Title = bookDto.Title,
            Description = bookDto.Description,
            Price = bookDto.Price,
            PublishDate = bookDto.PublishDate,
            Pages = bookDto.Pages,
            AuthorId = bookDto.AuthorId,
            PublisherId = bookDto.PublisherId
        };

        _bookServiceMock.Setup(service => service.AddAsync(It.IsAny<Book>())).ReturnsAsync(newBook);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Role, "Admin")
        }, "mock"));
        _bookController.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() { User = user }
        };

        // Act
        var result = await _bookController.CreateBook(bookDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        var returnBook = Assert.IsType<Book>(createdAtActionResult.Value);
        Assert.Equal(newBook.Id, returnBook.Id);
    }

    [Fact]
    public async Task UpdateBook_ReturnsNoContent_WhenBookIsUpdated()
    {
        // Arrange
        var bookDto = new BookDto
        {
            Title = "Updated Book",
            Description = "Updated Description",
            Price = 25.99m,
            PublishDate = new DateOnly(2023, 1, 1),
            Pages = 300,
            AuthorId = 1,
            PublisherId = 1
        };
        var existingBook = new Book
        {
            Id = 1,
            Title = "Existing Book",
            Description = "Existing Description",
            Price = 15.99m,
            PublishDate = new DateOnly(2021, 1, 1),
            Pages = 200,
            AuthorId = 1,
            PublisherId = 1
        };

        _bookServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(existingBook);
        _bookServiceMock.Setup(service => service.UpdateAsync(It.IsAny<Book>())).ReturnsAsync(existingBook);

        // Act
        var result = await _bookController.UpdateBook(1, bookDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateBook_ReturnsNotFound_WhenBookNotExists()
    {
        // Arrange
        var bookDto = new BookDto
        {
            Title = "Updated Book",
            Description = "Updated Description",
            Price = 25.99m,
            PublishDate = new DateOnly(2023, 1, 1),
            Pages = 300,
            AuthorId = 1,
            PublisherId = 1
        };

        _bookServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync((Book)null);

        // Act
        var result = await _bookController.UpdateBook(1, bookDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteBook_ReturnsNoContent_WhenBookIsDeleted()
    {
        // Arrange
        _bookServiceMock.Setup(service => service.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _bookController.DeleteBook(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteBook_ReturnsNotFound_WhenBookNotExists()
    {
        // Arrange
        _bookServiceMock.Setup(service => service.DeleteAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _bookController.DeleteBook(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
