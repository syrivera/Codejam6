using System.Net;

namespace CodeJam5b.Server.Tests.Integration;

public class ApiSmokeTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ApiSmokeTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetMeals_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/meals");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetProgress_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/progress");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
