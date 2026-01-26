using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeJam5b.Functions.Tests.TestUtilities;

public static class InMemoryDbContextFactory
{
    public static CalorieCounterContext Create(string databaseName)
    {
        var options = new DbContextOptionsBuilder<CalorieCounterContext>()
            .UseInMemoryDatabase(databaseName)
            .EnableSensitiveDataLogging()
            .Options;

        return new CalorieCounterContext(options);
    }
}
