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
            var progress = await _db.Progress.OrderBy(p => p.Id).FirstOrDefaultAsync();
        }

        catch (Exception err)
        {
            var res = req.CreateResponse(HttpStatusCode.InternalServerError);
            await res.WriteStringAsync($"Error retrieving progress: {err.Message}");
            return res;
        }

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

    private record UpdateProgressRequest(double CurrentWeight, double GoalWeight, int CurrentCalories, int GoalCalories);

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
            await bad.WriteStringAsync("Body must include CurrentWeight, GoalWeight, CurrentCalories, GoalCalories.");
            return bad;
        }

        var progress = await _db.Progress.OrderBy(p => p.Id).FirstOrDefaultAsync();
        if (progress is null)
        {
            progress = new UserProgress { Id = 1 };
            _db.Progress.Add(progress);
        }

        progress.CurrentWeight = body.CurrentWeight;
        progress.GoalWeight = body.GoalWeight;
        progress.CurrentCalories = body.CurrentCalories;
        progress.GoalCalories = body.GoalCalories;

        await _db.SaveChangesAsync();

        var ok = req.CreateResponse(HttpStatusCode.OK);
        await ok.WriteAsJsonAsync(progress);
        return ok;
    }
}