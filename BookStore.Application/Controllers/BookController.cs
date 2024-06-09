using BookStore.Application.Dtos;
using BookStore.Domain.Models;
using BookStore.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BookController(IBookService _bookService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook(BookDto bookDto)
    {
        var newBook = new Book
        {
            Title = bookDto.Title,
            Description = bookDto.Description,
            Price = bookDto.Price,
            PublishDate = bookDto.PublishDate,
            Pages = bookDto.Pages,
            AuthorId = bookDto.AuthorId,
            PublisherId = bookDto.PublisherId
        };
        newBook = await _bookService.AddAsync(newBook);
        return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook([FromRoute] int id, BookDto bookDto)
    {
        var existingBook = await _bookService.GetByIdAsync(id);
        if (existingBook == null)
        {
            return NotFound();
        }

        existingBook.Title = bookDto.Title;
        existingBook.Description = bookDto.Description;
        existingBook.Price = bookDto.Price;
        existingBook.PublishDate = bookDto.PublishDate;
        existingBook.Pages = bookDto.Pages;
        existingBook.AuthorId = bookDto.AuthorId;
        existingBook.PublisherId = bookDto.PublisherId;

        await _bookService.UpdateAsync(existingBook);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var result = await _bookService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
