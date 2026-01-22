using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class CalorieCounterContext : DbContext
{
    public CalorieCounterContext(DbContextOptions<CalorieCounterContext> options) : base(options) { }

    public DbSet<Meal> meals => Set<Meal>();
    public DbSet<UserProgress> progress => Set<UserProgress>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Meal>().ToTable("meals");
        modelBuilder.Entity<UserProgress>().ToTable("progress");
    }
}
