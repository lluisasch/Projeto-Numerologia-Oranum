namespace Oranum.Infrastructure.External;

public sealed class OpenAiOptions
{
    public const string SectionName = "OpenAI";

    public string ApiKey { get; init; } = string.Empty;
    public string BaseUrl { get; init; } = "https://api.openai.com/v1/";
    public string Model { get; init; } = "gpt-4.1-mini";
    public decimal Temperature { get; init; } = 0.9m;
    public int MaxRetries { get; init; } = 2;
    public int TimeoutSeconds { get; init; } = 45;
}
