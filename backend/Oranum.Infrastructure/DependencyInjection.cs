using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oranum.Application.Abstractions;
using Oranum.Infrastructure.Data;
using Oranum.Infrastructure.External;
using Oranum.Infrastructure.KnowledgeProviders;
using Oranum.Infrastructure.Persistence;

namespace Oranum.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpenAiOptions>(configuration.GetSection(OpenAiOptions.SectionName));

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? "Host=localhost;Port=5432;Database=oranum;Username=oranum;Password=oranum";

        services.AddDbContext<OranumDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(OranumDbContext).Assembly.FullName);
            });
        });

        services.AddHttpClient<IOpenAiMysticClient, OpenAiMysticClient>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<OpenAiOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        });

        services.AddScoped<IReadingPersistenceService, ReadingPersistenceService>();
        services.AddSingleton<IKnowledgeProvider, InternalMysticKnowledgeProvider>();

        return services;
    }
}
