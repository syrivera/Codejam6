using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Npgsql;

namespace Api.Functions;

public class DiagFunctions
{
  [Function("DiagDb")]
  public async Task<HttpResponseData> DiagDb(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "diag/db")] HttpRequestData req)
  {
    var conn = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

    if (string.IsNullOrWhiteSpace(conn))
    {
      var bad = req.CreateResponse(HttpStatusCode.InternalServerError);
      await bad.WriteStringAsync("Missing env var: ConnectionStrings__DefaultConnection");
      return bad;
    }

    try
    {
      await using var npg = new NpgsqlConnection(conn);
      await npg.OpenAsync();

      await using var cmd = new NpgsqlCommand("select 1", npg);
      var result = await cmd.ExecuteScalarAsync();

      var ok = req.CreateResponse(HttpStatusCode.OK);
      await ok.WriteStringAsync($"DB OK, select 1 => {result}");
      return ok;
    }
    catch (Exception ex)
    {
      var err = req.CreateResponse(HttpStatusCode.InternalServerError);
      await err.WriteStringAsync($"DB FAIL: {ex.GetType().Name}: {ex.Message}");
      return err;
    }
  }
}