using CodeJam5b.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeJam5b.Server.Tests.TestUtilities;

public static class DbContextFactory
{
    public static CalorieCounterContext CreateInMemory(string databaseName)
    {
        var options = new DbContextOptionsBuilder<CalorieCounterContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        var context = new CalorieCounterContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}
