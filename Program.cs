using Microsoft.Data.SqlClient;
using MyApp.Infra;
using MyApp.Middleware;
using MyApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI registrations
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<IPromptRepository, PromptRepository>();
builder.Services.AddScoped<IPromptService, PromptService>();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.MapGet("/health/db", (IDbConnectionFactory dbFactory) =>
{
    try
    {
        using var conn = dbFactory.CreateConnection();
        return Results.Ok(new { status = "ok", db = "connected" });
    }
    catch (Exception)
    {
        return Results.StatusCode(503);
    }
});

app.Run();
