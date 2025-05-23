using LugxApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LugxApp.Contexts;

public class LugxDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Game> Games { get; set; }

    public LugxDbContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
