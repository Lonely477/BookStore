using BookStore.Domain.Models;
using BookStore.Infrastructure;

namespace BookStore.Application.Services;

public class DataSeederService(ApplicationDbContext _context)
{
    public void SeedData()
    {
        SeedUsers();
        SeedPublishers();
        SeedAuthorsAndBooks();
    }

    private void SeedUsers()
    {
        if (!_context.Users.Any())
        {
            List<User> users =
            [
                new()
                {
                    Username = "admin",
                    PasswordHash = "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", // "admin"
                    Role = "Admin"
                },
                new()
                {
                    Username = "user",
                    PasswordHash = "BPiZbadjt6lpsQKO4wB1aerzpjVIbdqyEdUSyFud+Ps=", // "user"
                    Role = "User"
                }
            ];

            _context.Users.AddRange(users);
            _context.SaveChanges();
        }
    }

    private void SeedPublishers()
    {
        if (!_context.Publishers.Any())
        {
            List<Publisher> publishers =
            [
                new(){ Name = "Penguin Random House" },
                new(){ Name = "HarperCollins" },
                new(){ Name = "Simon & Schuster" },
                new(){ Name = "Hachette Book Group" },
                new(){ Name = "Macmillan Publishers" }
            ];

            _context.Publishers.AddRange(publishers);
            _context.SaveChanges();
        }
    }

    private void SeedAuthorsAndBooks()
    {
        if (!_context.Authors.Any())
        {
            List<Author> authors =
            [
                new() { FirstName = "George", LastName = "Orwell", Country = Country.GB },
                new() { FirstName = "J.K.", LastName = "Rowling", Country = Country.GB },
                new() { FirstName = "Ernest", LastName = "Hemingway", Country = Country.US },
                new() { FirstName = "Jane", LastName = "Austen", Country = Country.GB },
                new() { FirstName = "Mark", LastName = "Twain", Country = Country.US }
            ];

            _context.Authors.AddRange(authors);
            _context.SaveChanges();

            List<Book> books =
            [
                new() { Title = "1984", Description = "Dystopian novel", Price = 9.99m, PublishDate = new DateOnly(1949, 6, 8), Pages = 328, AuthorId = 1, PublisherId = 1 },
                new() { Title = "Animal Farm", Description = "Political satire", Price = 7.99m, PublishDate = new DateOnly(1945, 8, 17), Pages = 112, AuthorId = 1, PublisherId = 1 },
                new() { Title = "Harry Potter and the Philosopher's Stone", Description = "Fantasy novel", Price = 12.99m, PublishDate = new DateOnly(1997, 6, 26), Pages = 223, AuthorId = 2, PublisherId = 2 },
                new() { Title = "Harry Potter and the Chamber of Secrets", Description = "Fantasy novel", Price = 12.99m, PublishDate = new DateOnly(1998, 7, 2), Pages = 251, AuthorId = 2, PublisherId = 2 },
                new() { Title = "The Old Man and the Sea", Description = "Novel", Price = 8.99m, PublishDate = new DateOnly(1952, 9, 1), Pages = 127, AuthorId = 3, PublisherId = 3 },
                new() { Title = "A Farewell to Arms", Description = "War novel", Price = 10.99m, PublishDate = new DateOnly(1929, 9, 27), Pages = 355, AuthorId = 3, PublisherId = 3 },
                new() { Title = "Pride and Prejudice", Description = "Romantic novel", Price = 6.99m, PublishDate = new DateOnly(1813, 1, 28), Pages = 279, AuthorId = 4, PublisherId = 4 },
                new() { Title = "Emma", Description = "Novel", Price = 7.99m, PublishDate = new DateOnly(1815, 12, 23), Pages = 474, AuthorId = 4, PublisherId = 4 },
                new() { Title = "Adventures of Huckleberry Finn", Description = "Novel", Price = 9.99m, PublishDate = new DateOnly(1884, 12, 10), Pages = 366, AuthorId = 5, PublisherId = 5 },
                new() { Title = "The Adventures of Tom Sawyer", Description = "Novel", Price = 8.99m, PublishDate = new DateOnly(1876, 4, 30), Pages = 274, AuthorId = 5, PublisherId = 5 }
            ];

            _context.Books.AddRange(books);
            _context.SaveChanges();
        }
    }


}
