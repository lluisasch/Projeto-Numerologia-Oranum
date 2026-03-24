using System.Threading.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using Oranum.Application.Abstractions;
using Oranum.Application.Services;
using Oranum.Application.Validators;
using Oranum.Domain.Services;
using Oranum.Infrastructure;
using Oranum.Infrastructure.Data;
using Oranum.Api.Middleware;

namespace Oranum.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddProblemDetails();
        builder.Services.AddHealthChecks();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddScoped<IReadingService, ReadingService>();
        builder.Services.AddScoped<ReadingRequestValidator>();
        builder.Services.AddSingleton<NumerologyCalculator>();
        builder.Services.AddSingleton<AstrologyCalculator>();
        builder.Services.AddSingleton<CompatibilityCalculator>();

        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
            ?? ["http://localhost:5173"];

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("oranum-cors", policy =>
            {
                policy.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        var permitLimit = builder.Configuration.GetValue<int?>("RateLimiting:PermitLimit") ?? 30;
        var windowSeconds = builder.Configuration.GetValue<int?>("RateLimiting:WindowSeconds") ?? 60;
        var queueLimit = builder.Configuration.GetValue<int?>("RateLimiting:QueueLimit") ?? 5;

        builder.Services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.AddFixedWindowLimiter("api", limiterOptions =>
            {
                limiterOptions.PermitLimit = permitLimit;
                limiterOptions.Window = TimeSpan.FromSeconds(windowSeconds);
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = queueLimit;
            });
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<OranumDbContext>();
            dbContext.Database.Migrate();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseCors("oranum-cors");
        app.UseRateLimiter();

        app.MapControllers().RequireRateLimiting("api");

        app.Run();
    }
}

