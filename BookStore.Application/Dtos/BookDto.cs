using System.ComponentModel.DataAnnotations;

namespace BookStore.Application.Dtos;

public class BookDto
{
    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(500, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 500 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    [Required]
    public DateOnly PublishDate { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Pages must be greater than 0.")]
    public int Pages { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "AuthorId must be a positive integer.")]
    public int AuthorId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "PublisherId must be a positive integer.")]
    public int PublisherId { get; set; }
}