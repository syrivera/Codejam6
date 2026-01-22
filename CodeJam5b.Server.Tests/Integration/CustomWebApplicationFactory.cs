using CodeJam5b.Server.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeJam5b.Server.Tests.Integration;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace the production DB context with an in-memory database for tests.
            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<CalorieCounterContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<CalorieCounterContext>(options =>
                options.UseInMemoryDatabase("CalorieCounter_TestDb"));

            using var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CalorieCounterContext>();
            db.Database.EnsureCreated();
        });
    }
}
