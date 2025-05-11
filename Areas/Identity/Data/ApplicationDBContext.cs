using EventFind.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace EventFind.Areas.Identity.Data;

public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
        : base(options)
    {
    }
    public DbSet<Event> Events { get; set; }
    public DbSet<Category> Categories { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Event>()
                        .HasOne(d => d.Category)
                        .WithMany()
                        .HasForeignKey(m => m.Category_ID)
                        .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Category>()
                        .HasOne(d => d.User)
                        .WithMany()
                        .HasForeignKey(m => m.UserId)
                        .OnDelete(DeleteBehavior.NoAction);

        base.OnModelCreating(builder);
    }
}
