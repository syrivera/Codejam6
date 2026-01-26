using System.Net;
using System.Text;
using Api.Functions;
using Api.Models;
using CodeJam5b.Functions.Tests.TestUtilities;
using Microsoft.Azure.Functions.Worker.Http;

namespace CodeJam5b.Functions.Tests.Functions;

public class MealsFunctionsTests
{
    [Fact]
    public async Task MealsCreate_ReturnsBadRequest_WhenBodyInvalid()
    {
        using var db = InMemoryDbContextFactory.Create(nameof(MealsCreate_ReturnsBadRequest_WhenBodyInvalid));
        var sut = new MealsFunctions(db);

        var ctx = new FakeFunctionContext();
        var req = new FakeHttpRequestData(
            ctx,
            new Uri("http://localhost/api/meals"),
            new MemoryStream(Encoding.UTF8.GetBytes("not-json")));

        var res = await sut.MealsCreate(req);

        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task MealsCreate_CreatesMeal_ReturnsCreated()
    {
        using var db = InMemoryDbContextFactory.Create(nameof(MealsCreate_CreatesMeal_ReturnsCreated));
        var sut = new MealsFunctions(db);

        var payload = "{\"name\":\"Banana\",\"calories\":105,\"date\":\"2024-01-01T00:00:00Z\"}";
        var ctx = new FakeFunctionContext();
        var req = new FakeHttpRequestData(
            ctx,
            new Uri("http://localhost/api/meals"),
            new MemoryStream(Encoding.UTF8.GetBytes(payload)));

        var res = await sut.MealsCreate(req);

        Assert.Equal(HttpStatusCode.Created, res.StatusCode);
        Assert.Single(db.Meals);
        Assert.Equal("Banana", db.Meals.Single().Name);
    }

    [Fact]
    public async Task MealsSearch_ReturnsBadRequest_WhenMissingNameQuery()
    {
        using var db = InMemoryDbContextFactory.Create(nameof(MealsSearch_ReturnsBadRequest_WhenMissingNameQuery));
        var sut = new MealsFunctions(db);

        var ctx = new FakeFunctionContext();
        var req = new FakeHttpRequestData(
            ctx,
            new Uri("http://localhost/api/meals/search"),
            new MemoryStream());

        var res = await sut.MealsSearch(req);

        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task MealsSearch_ReturnsOk_WithMatchingResults()
    {
        using var db = InMemoryDbContextFactory.Create(nameof(MealsSearch_ReturnsOk_WithMatchingResults));
        db.Meals.AddRange(
            new Meal { Id = 1, Name = "Chicken Salad", Calories = 400, Date = new DateTime(2024, 1, 1) },
            new Meal { Id = 2, Name = "Apple", Calories = 95, Date = new DateTime(2024, 1, 2) });
        db.SaveChanges();

        var sut = new MealsFunctions(db);

        var ctx = new FakeFunctionContext();
        var req = new FakeHttpRequestData(
            ctx,
            new Uri("http://localhost/api/meals/search?name=chicken"),
            new MemoryStream());

        var res = await sut.MealsSearch(req);

        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }
}
