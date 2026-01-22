using CodeJam5b.Server.Tests.TestUtilities;

namespace CodeJam5b.Server.Tests.Data;

public class CalorieCounterContextTests
{
    [Fact]
    public void EnsureCreated_SeedsMeals()
    {
        using var db = DbContextFactory.CreateInMemory(nameof(EnsureCreated_SeedsMeals));
        Assert.True(db.Meals.Any());
    }
}
