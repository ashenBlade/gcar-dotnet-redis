using GcraRateLimit.Producer;
using GcraRateLimit.Producer.HostedServices;
using GcraRateLimit.Producer.Options;
using GcraRateLimit.RateLimiter;
using Microsoft.Extensions.Options;
using Pipelines.Sockets.Unofficial;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var options = sp.GetRequiredService<IOptions<RedisOptions>>().Value;
    return ConnectionMultiplexer.Connect(options.Host);
});
builder.Services
       .AddOptions<RedisOptions>()
       .Bind(builder.Configuration.GetRequiredSection("REDIS"))
       .ValidateDataAnnotations()
       .ValidateOnStart();

builder.Services.AddSingleton<IRateLimiter, RedisGcraRateLimiter>();

builder.Services
       .AddOptions<RateLimiterOptions>()
       .Bind(builder.Configuration.GetRequiredSection("RATE_LIMIT"))
       .ValidateDataAnnotations()
       .ValidateOnStart();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
       .AddOptions<RequestSenderOptions>()
       .Bind(builder.Configuration)
       .ValidateDataAnnotations()
       .ValidateOnStart();

builder.Services.AddHostedService<RequestSenderBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();