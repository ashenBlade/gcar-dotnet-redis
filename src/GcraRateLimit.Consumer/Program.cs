using GcraRateLimit.Producer;
using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.Services
       .AddOpenTelemetry()
       .WithMetrics(m =>
        {
            m.AddPrometheusExporter();
            m.AddMeter(Counters.AppMeter.Name);
        });

var app = builder.Build();

app.UseOpenTelemetryPrometheusScrapingEndpoint("/metrics");
app.MapGet("/rate-limit", async () =>
{
    Counters.SuccessRequests.Add(1);
    if (Random.Shared.Next(0, 2) == 0)
    {
        return Results.Ok();
    }
    await Task.Delay(Random.Shared.Next(0, 50));
    return Results.Ok();
});

try
{
    app.Run();
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}