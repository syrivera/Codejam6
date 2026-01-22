using CodeJam5b.Server.Controllers;
using CodeJam5b.Server.Tests.TestUtilities;
using Microsoft.AspNetCore.Mvc;

namespace CodeJam5b.Server.Tests.Controllers;

public class CalorieCounterControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsSeededMeals()
    {
        using var db = DbContextFactory.CreateInMemory(nameof(GetAll_ReturnsSeededMeals));
        var controller = new CalorieCounterController(db);

        var result = await controller.GetAll();

        var ok = Assert.IsType<ActionResult<IEnumerable<MealEntry>>>(result);
        var meals = Assert.IsAssignableFrom<IEnumerable<MealEntry>>(ok.Value);
        Assert.NotEmpty(meals);
    }

    [Fact]
    public async Task Create_AddsMeal_AndReturnsCreatedAtAction()
    {
        using var db = DbContextFactory.CreateInMemory(nameof(Create_AddsMeal_AndReturnsCreatedAtAction));
        var controller = new CalorieCounterController(db);

        var body = new MealEntry
        {
            Name = "Test Meal",
            Calories = 500,
            Carbs = 50,
            Fat = 10,
            Protein = 25
        };

        var result = await controller.Create(body);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdBody = Assert.IsType<MealEntry>(created.Value);
        Assert.False(string.IsNullOrWhiteSpace(createdBody.Id));
        Assert.Equal("Test Meal", createdBody.Name);

        var mealInDb = await db.Meals.FindAsync(createdBody.Id);
        Assert.NotNull(mealInDb);
        Assert.Equal("Test Meal", mealInDb!.MealName);
        Assert.Equal(500, mealInDb.Calories);
    }

    [Fact]
    public async Task Delete_RemovesMeal_AndReturnsNoContent()
    {
        using var db = DbContextFactory.CreateInMemory(nameof(Delete_RemovesMeal_AndReturnsNoContent));
        var controller = new CalorieCounterController(db);

        var created = await controller.Create(new MealEntry
        {
            Name = "Delete Me",
            Calories = 100,
            Carbs = 10,
            Fat = 1,
            Protein = 5
        });

        var createdResult = Assert.IsType<CreatedAtActionResult>(created.Result);
        var createdBody = Assert.IsType<MealEntry>(createdResult.Value);
        var id = createdBody.Id;
        Assert.False(string.IsNullOrWhiteSpace(id));

        var deleteResult = await controller.Delete(id!);

        Assert.IsType<NoContentResult>(deleteResult);
        var mealInDb = await db.Meals.FindAsync(id);
        Assert.Null(mealInDb);
    }
}
