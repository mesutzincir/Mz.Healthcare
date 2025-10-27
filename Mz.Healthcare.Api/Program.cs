using Mz.Healthcare.Api.Data;
using Mz.Healthcare.Api.Extensions;
using Mz.Healthcare.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Seed in-memory database
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    TestDataSeeder.Seed(dbContext);
}

app.MapControllers();
app.Logger.LogInformation("Starting {Application}...", app.Environment.ApplicationName);
await app.RunAsync();