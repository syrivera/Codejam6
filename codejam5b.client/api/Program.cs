using Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        var conn = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        
        Console.WriteLine("Database connection string:");
        Console.WriteLine(conn);

        if (string.IsNullOrWhiteSpace(conn))
            throw new InvalidOperationException("Missing ConnectionStrings__DefaultConnection.");

        services.AddDbContext<CalorieCounterContext>(options => options.UseNpgsql(conn));
    })
    .Build();

host.Run();