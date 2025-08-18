using Microsoft.EntityFrameworkCore;
using RoboticForkliftControlSystem.Api.Abstractions;
using RoboticForkliftControlSystem.Api.Constants;
using RoboticForkliftControlSystem.Api.Data;
using RoboticForkliftControlSystem.Api.Services;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IForkliftService, ForkliftService>();
builder.Services.AddScoped<IMovementService, MovementService>();

//NOTE: for now, we only sink into console. We can sink into Application Insights later by a few configs and new lib
Log.Logger = new LoggerConfiguration().Enrich.FromLogContext().WriteTo.Console().CreateLogger();

// hook Serilog into ASP.NET Core logging pipeline
builder.Host.UseSerilog();

var allowedOrigins =
    builder.Configuration.GetSection(AppConfig.AllowedOrigins).Get<string[]>()
    ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: AppConfig.CorsPolicy,
        policy =>
        {
            policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        }
    );
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString(AppConfig.Default);
    options.UseNpgsql(cs);

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseCors(AppConfig.CorsPolicy);
app.MapControllers();

app.MapHealthChecks("/healthz");
app.Run();
