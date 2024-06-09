using BookStore.Application.Dtos;
using BookStore.Domain.Models;
using BookStore.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthorController(IAuthorService _authorService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAuthors()
    {
        IEnumerable<Author> authors = await _authorService.GetAllAsync();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuthorById(int id)
    {
        Author? author = await _authorService.GetByIdAsync(id);
        if (author is null)
        {
            return NotFound();
        }
        return Ok(author);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateAuthor(AuthorDto authorDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Author newAuthor = new()
        {
            FirstName = authorDto.FirstName,
            LastName = authorDto.LastName,
            Country = authorDto.Country
        };
        newAuthor = await _authorService.AddAsync(newAuthor);
        return CreatedAtAction(nameof(GetAuthorById), new { id = newAuthor.Id }, newAuthor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuthor([FromRoute] int id, AuthorDto authorDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Author? existingAuthor = await _authorService.GetByIdAsync(id);
        if (existingAuthor is null)
        {
            return NotFound();
        }

        existingAuthor.FirstName = authorDto.FirstName;
        existingAuthor.LastName = authorDto.LastName;
        existingAuthor.Country = authorDto.Country;

        await _authorService.UpdateAsync(existingAuthor);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        var result = await _authorService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
