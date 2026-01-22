using System.Net;
using System.Text.Json;
using Api.Data;
using Api.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;

namespace Api.Functions;

public class MealsFunctions
{
    private readonly CalorieCounterContext _db;
    public MealsFunctions(CalorieCounterContext db) => _db = db;

    [Function("MealsSearch")]
    public async Task<HttpResponseData> MealsSearch(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "meals/search")] HttpRequestData req)
    {
        var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
        var name = (query["name"] ?? "").Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            var bad = req.CreateResponse(HttpStatusCode.BadRequest);
            await bad.WriteStringAsync("Query parameter 'name' is required.");
            return bad;
        }

        var results = await _db.meals
            .Where(m => EF.Functions.ILike(m.name, $"%{name}%"))
            .OrderByDescending(m => m.meal_id)
            .Take(50)
            .ToListAsync();

        var ok = req.CreateResponse(HttpStatusCode.OK);
        await ok.WriteAsJsonAsync(results);
        return ok;
    }

    private record CreateMealRequest(string name, int calories, DateTime date);

    [Function("MealsCreate")]
    public async Task<HttpResponseData> MealsCreate(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "meals")] HttpRequestData req)
    {
        CreateMealRequest? body;
        try
        {
            body = await JsonSerializer.DeserializeAsync<CreateMealRequest>(
                req.Body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch
        {
            body = null;
        }

        if (body is null || string.IsNullOrWhiteSpace(body.name) || body.calories < 0)
        {
            var bad = req.CreateResponse(HttpStatusCode.BadRequest);
            await bad.WriteStringAsync("Body must include Name, Calories, Date.");
            return bad;
        }

        var meal = new Meal
        {
            name = body.name.Trim(),
            calories = body.calories,
            date = body.date == default ? DateTime.Today : body.Date
        };

        _db.meals.Add(meal);
        await _db.SaveChangesAsync();

        var created = req.CreateResponse(HttpStatusCode.Created);
        await created.WriteAsJsonAsync(meal);
        return created;
    }
}