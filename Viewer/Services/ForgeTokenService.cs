using Newtonsoft.Json;
using Viewer.Configuration;

namespace Viewer.Services;

public class ForgeTokenService(AuthConfig config, IHttpClientFactory factory) : IForgeTokenService
{
    public async Task<string?> GetForgeToken()
    {
        var dict = new Dictionary<string, string>
        {
            { "client_id", config.ClientId },
            { "client_secret", config.ClientSecret },
            { "grant_type", "client_credentials" },
            { "scope", "viewables:read" }
        };

        using var httpClient = factory.CreateClient(nameof(ForgeTokenService));
        using var response = await httpClient.PostAsync(
            "/authentication/v2/token", new FormUrlEncodedContent(dict));
        await using var responseStream = await response.Content.ReadAsStreamAsync();
        using var streamReader = new StreamReader(responseStream);
        await using var jsonTextReader = new JsonTextReader(streamReader);
        if (responseStream.Length > 0)
        {
            var tokenResponse = JsonSerializer.CreateDefault().Deserialize<TokenResponse>(jsonTextReader);
            return tokenResponse?.access_token;
        }

        return default;
    }
}

public class TokenResponse
{
    public string access_token;
}