using BookStore.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Application.Dtos;

public class AuthorDto
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Country is required")]
    public Country Country { get; set; }
}
