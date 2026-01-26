using System.Net;
using System.Text;
using Api.Functions;
using CodeJam5b.Functions.Tests.TestUtilities;

namespace CodeJam5b.Functions.Tests.Functions;

public class ProgressFunctionsTests
{
    [Fact]
    public async Task ProgressPut_ReturnsBadRequest_WhenBodyInvalid()
    {
        using var db = InMemoryDbContextFactory.Create(nameof(ProgressPut_ReturnsBadRequest_WhenBodyInvalid));
        var sut = new ProgressFunctions(db);

        var ctx = new FakeFunctionContext();
        var req = new FakeHttpRequestData(
            ctx,
            new Uri("http://localhost/api/progress"),
            new MemoryStream(Encoding.UTF8.GetBytes("not-json")));

        var res = await sut.ProgressPut(req);

        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    [Fact]
    public async Task ProgressPut_CreatesProgress_WhenMissing_ReturnsOk()
    {
        using var db = InMemoryDbContextFactory.Create(nameof(ProgressPut_CreatesProgress_WhenMissing_ReturnsOk));
        var sut = new ProgressFunctions(db);

        var payload = "{\"currentWeight\":180,\"goalWeight\":170,\"currentCalories\":1000,\"goalCalories\":2000}";
        var ctx = new FakeFunctionContext();
        var req = new FakeHttpRequestData(
            ctx,
            new Uri("http://localhost/api/progress"),
            new MemoryStream(Encoding.UTF8.GetBytes(payload)));

        var res = await sut.ProgressPut(req);

        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        Assert.Single(db.Progress);
    }
}
