using Microsoft.EntityFrameworkCore;
using WebShell.Api.Models;
using WebShell.Api.Services;

namespace WebShell.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<CommandHistory> CommandHistories => Set<CommandHistory>();
}