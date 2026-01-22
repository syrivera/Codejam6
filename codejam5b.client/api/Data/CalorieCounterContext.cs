using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class CalorieCounterContext : DbContext
{
    public CalorieCounterContext(DbContextOptions<CalorieCounterContext> options) : base(options) { }

    public DbSet<Meal> Meals { get; set; }
    public DbSet<UserProgress> Progress { get; set; }
}