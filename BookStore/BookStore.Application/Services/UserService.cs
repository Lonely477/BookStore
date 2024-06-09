using BookStore.Domain.Models;
using BookStore.Domain.Services;
using BookStore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BookStore.Application.Services;

public class UserService(ApplicationDbContext context) : IUserService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<User?> Authenticate(string username, string password)
    {
        User? user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

        if (user is null || !VerifyPasswordHash(password, user.PasswordHash))
            return null;

        return user;
    }

    public async Task<User?> Register(string username, string password)
    {
        if (await _context.Users.AnyAsync(u => u.Username == username))
            return null;

        User user = new()
        {
            Username = username,
            PasswordHash = HashPassword(password),
            Role = "User" // default role, admin account is pre-created in the database
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    private static string HashPassword(string password)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private static bool VerifyPasswordHash(string password, string storedHash)
    {
        string hashOfInput = HashPassword(password);
        return StringComparer.OrdinalIgnoreCase.Compare(hashOfInput, storedHash) == 0;
    }
}
