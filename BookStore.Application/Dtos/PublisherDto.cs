using System.ComponentModel.DataAnnotations;

namespace BookStore.Application.Dtos;

public class PublisherDto
{
    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters.")]
    public string Name { get; set; } = string.Empty;
}
