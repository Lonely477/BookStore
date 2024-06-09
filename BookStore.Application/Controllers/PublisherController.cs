using BookStore.Application.Dtos;
using BookStore.Domain.Models;
using BookStore.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class PublisherController : ControllerBase
{
    private readonly IPublisherService _publisherService;

    public PublisherController(IPublisherService publisherService)
    {
        _publisherService = publisherService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPublishers()
    {
        List<Publisher> publishers = await _publisherService.GetAllAsync();
        return Ok(publishers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPublisherById(int id)
    {
        Publisher? publisher = await _publisherService.GetByIdAsync(id);
        if (publisher is null)
        {
            return NotFound();
        }
        return Ok(publisher);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePublisher(PublisherDto publisherDto)
    {
        Publisher newPublisher = new()
        {
            Name = publisherDto.Name
        };
        newPublisher = await _publisherService.AddAsync(newPublisher);
        return CreatedAtAction(nameof(GetPublisherById), new { id = newPublisher.Id }, newPublisher);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePublisher([FromRoute] int id, PublisherDto publisherDto)
    {
        Publisher? existingPublisher = await _publisherService.GetByIdAsync(id);

        if (existingPublisher is null)
        {
            return NotFound();
        }

        existingPublisher.Name = publisherDto.Name;

        await _publisherService.UpdateAsync(existingPublisher);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePublisher(int id)
    {
        var result = await _publisherService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
