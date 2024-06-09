using BookStore.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{

}
