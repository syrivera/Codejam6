using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

// Minimal DbContext used by function/unit tests.
public class CalorieCounterContext : DbContext
{
    public CalorieCounterContext(DbContextOptions<CalorieCounterContext> options) : base(options) { }

    public DbSet<Meal> Meals => Set<Meal>();
    public DbSet<UserProgress> Progress => Set<UserProgress>();
}
