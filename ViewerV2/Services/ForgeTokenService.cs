using System.Text;
using Newtonsoft.Json;
using Viewer.Configuration;

namespace ViewerV2.Services;

public class ForgeTokenService : IForgeTokenService
{
    private readonly AuthConfig config;
    private readonly IHttpClientFactory factory;

    public ForgeTokenService(AuthConfig config, IHttpClientFactory factory)
    {
        this.config = config;
        this.factory = factory;
    }

    public async Task<string?> GetForgeToken()
    {
        var dict = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "scope", "code:all data:write data:read bucket:create bucket:delete bucket:read" }
        };

        var base64Credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{config.ClientId}:{config.ClientSecret}"));

        using var httpClient = factory.CreateClient(nameof(ForgeTokenService));
        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64Credentials);
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