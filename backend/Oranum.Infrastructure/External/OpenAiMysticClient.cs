using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oranum.Application.Abstractions;
using Oranum.Application.Models;

namespace Oranum.Infrastructure.External;

public sealed class OpenAiMysticClient : IOpenAiMysticClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    private readonly OpenAiOptions _options;
    private readonly ILogger<OpenAiMysticClient> _logger;

    public OpenAiMysticClient(HttpClient httpClient, IOptions<OpenAiOptions> options, ILogger<OpenAiMysticClient> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<AiStructuredResult<T>> GenerateJsonAsync<T>(string systemPrompt, string userPrompt, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            throw new InvalidOperationException("OpenAI API key nao configurada.");
        }

        for (var attempt = 0; attempt <= _options.MaxRetries; attempt++)
        {
            try
            {
                using var request = BuildRequest(systemPrompt, userPrompt);
                using var response = await _httpClient.SendAsync(request, cancellationToken);
                var rawBody = await response.Content.ReadAsStringAsync(cancellationToken);
                response.EnsureSuccessStatusCode();

                using var document = JsonDocument.Parse(rawBody);
                var model = document.RootElement.TryGetProperty("model", out var modelNode)
                    ? modelNode.GetString()
                    : _options.Model;
                var content = document.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString();

                var normalizedJson = ExtractJson(content);
                var payload = JsonSerializer.Deserialize<T>(normalizedJson, JsonOptions);
                return new AiStructuredResult<T>(payload, model, normalizedJson);
            }
            catch (Exception exception) when (attempt < _options.MaxRetries)
            {
                var delay = TimeSpan.FromMilliseconds(500 * (attempt + 1));
                _logger.LogWarning(exception, "Falha temporaria ao consultar OpenAI. Nova tentativa em {Delay}ms.", delay.TotalMilliseconds);
                await Task.Delay(delay, cancellationToken);
            }
        }

        throw new InvalidOperationException("Nao foi possivel obter resposta estruturada da OpenAI apos as tentativas configuradas.");
    }

    private HttpRequestMessage BuildRequest(string systemPrompt, string userPrompt)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "chat/completions");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);
        request.Content = JsonContent.Create(new
        {
            model = _options.Model,
            temperature = _options.Temperature,
            response_format = new { type = "json_object" },
            messages = new object[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = userPrompt }
            }
        });

        return request;
    }

    private static string ExtractJson(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return "{}";
        }

        var trimmed = content.Trim();
        if (trimmed.StartsWith("```", StringComparison.Ordinal))
        {
            var lines = trimmed.Split('\n')
                .Where(line => !line.TrimStart().StartsWith("```", StringComparison.Ordinal))
                .ToArray();
            trimmed = string.Join('\n', lines).Trim();
        }

        var startIndex = trimmed.IndexOf('{');
        var endIndex = trimmed.LastIndexOf('}');
        if (startIndex >= 0 && endIndex > startIndex)
        {
            return trimmed[startIndex..(endIndex + 1)];
        }

        return trimmed;
    }
}
