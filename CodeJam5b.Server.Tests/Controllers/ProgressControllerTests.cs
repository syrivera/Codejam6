using CodeJam5b.Server.Controllers;
using CodeJam5b.Server.Tests.TestUtilities;
using Microsoft.AspNetCore.Mvc;

namespace CodeJam5b.Server.Tests.Controllers;

public class ProgressControllerTests
{
    [Fact]
    public async Task Get_WhenProgressExists()
    {
        using var db = DbContextFactory.CreateInMemory(nameof(Get_WhenProgressExists));
        var controller = new ProgressController(db);

        var result = await controller.Get();

        Assert.Null(result.Result);
        var progress = Assert.IsType<ProgressData>(result.Value);
        Assert.False(string.IsNullOrWhiteSpace(progress.Id));
    }

    [Fact]
    public async Task Update_WhenProgressExists()
    {
        using var db = DbContextFactory.CreateInMemory(nameof(Update_WhenProgressExists));
        var controller = new ProgressController(db);

        // seeded via CalorieCounterContext.OnModelCreating
        Assert.Equal(1, db.Progress.Count());
        var seededId = db.Progress.Single().ProgressId;

        var body = new ProgressData
        {
            CurrentWeight = 200,
            TargetWeight = 180,
            TargetDailyCalories = 2200,
            TargetDailyCarbs = 200,
            TargetDailyFat = 70,
            TargetDailyProtein = 150
        };

        var result = await controller.Update(body);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal(1, db.Progress.Count());

        var saved = db.Progress.Single();
        Assert.Equal(seededId, saved.ProgressId);
        Assert.Equal(200, saved.CurrentWeight);
        Assert.Equal(180, saved.TargetWeight);
        Assert.Equal(2200, saved.TargetCals);
        Assert.Equal(200, saved.TargetCarbs);
        Assert.Equal(70, saved.TargetFat);
        Assert.Equal(150, saved.TargetProtein);
    }
}
