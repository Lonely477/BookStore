namespace BookStore.Domain.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateOnly PublishDate { get; set; }
    public int Pages { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; } = default!;
    public int PublisherId { get; set; }
    public Publisher Publisher { get; set; } = default!;
}
