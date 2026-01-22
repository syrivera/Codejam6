using System.Net;
using System.Text.Json;
using Api.Data;
using Api.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;

namespace Api.Functions;

public class ProgressFunctions
{
    private readonly CalorieCounterContext _db;
    public ProgressFunctions(CalorieCounterContext db) => _db = db;

    [Function("ProgressGet")]
    public async Task<HttpResponseData> ProgressGet(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "progress")] HttpRequestData req)
    {
        try
        {
            var progress = await _db.progress.OrderBy(p => p.progress_id).FirstOrDefaultAsync();
            if (progress is null)
            {
                var nf = req.CreateResponse(HttpStatusCode.NotFound);
                await nf.WriteStringAsync("No progress record found.");
                return nf;
            }

            var ok = req.CreateResponse(HttpStatusCode.OK);
            await ok.WriteAsJsonAsync(progress);
            return ok;
        }
        
        catch (Exception err)
        {
            var res = req.CreateResponse(HttpStatusCode.InternalServerError);
            await res.WriteStringAsync($"Error retrieving progress: {err.Message}");
            return res;
        }
    }

    private record UpdateProgressRequest(double consumedCalories, double consumedCarbs, int consumedFat, int consumedProtein);

    [Function("ProgressPut")]
    public async Task<HttpResponseData> ProgressPut(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "progress")] HttpRequestData req)
    {
        UpdateProgressRequest? body;
        try
        {
            body = await JsonSerializer.DeserializeAsync<UpdateProgressRequest>(
                req.Body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch
        {
            body = null;
        }

        if (body is null)
        {
            var bad = req.CreateResponse(HttpStatusCode.BadRequest);
            await bad.WriteStringAsync("Body must include Consumed cals, carbs, fat, & protein.");
            return bad;
        }

        var progress = await _db.progress.OrderBy(p => p.progress_id).FirstOrDefaultAsync();
        if (progress is null)
        {
            progress = new UserProgress { progress_id = 1 };
            _db.progress.Add(progress);
        }

        progress.consumed_cals = body.consumedCalories;
        progress.consumed_carbs = body.consumedCarbs;
        progress.consumed_fat = body.consumedFat;
        progress.consumed_protein = body.consumedProtein;

        await _db.SaveChangesAsync();

        var ok = req.CreateResponse(HttpStatusCode.OK);
        await ok.WriteAsJsonAsync(progress);
        return ok;
    }
}