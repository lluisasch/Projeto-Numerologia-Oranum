using Npgsql;

namespace Oranum.Infrastructure.Data;

internal static class ConnectionStringResolver
{
    private const string DefaultConnectionString =
        "Host=localhost;Port=5432;Database=oranum;Username=oranum;Password=oranum";

    public static string Resolve(string? connectionValue)
    {
        if (string.IsNullOrWhiteSpace(connectionValue))
        {
            return DefaultConnectionString;
        }

        if (connectionValue.StartsWith("Host=", StringComparison.OrdinalIgnoreCase))
        {
            return connectionValue;
        }

        if (!Uri.TryCreate(connectionValue, UriKind.Absolute, out var uri))
        {
            return connectionValue;
        }

        if (!string.Equals(uri.Scheme, "postgres", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(uri.Scheme, "postgresql", StringComparison.OrdinalIgnoreCase))
        {
            return connectionValue;
        }

        var userInfo = uri.UserInfo.Split(':', 2);
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.Port > 0 ? uri.Port : 5432,
            Database = uri.AbsolutePath.Trim('/'),
            Username = Uri.UnescapeDataString(userInfo.ElementAtOrDefault(0) ?? string.Empty),
            Password = Uri.UnescapeDataString(userInfo.ElementAtOrDefault(1) ?? string.Empty)
        };

        return builder.ConnectionString;
    }
}
