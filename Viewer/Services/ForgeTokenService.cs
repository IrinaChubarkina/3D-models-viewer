namespace Viewer.Services;

public class ForgeTokenService
{
    public async Task<string> GetForgeTokenAsync()
    {
        var dict = new Dictionary<string, string>();
        dict.Add("client_id", "<client id>");
        dict.Add("client_secret", "<client secret>");
        dict.Add("grant_type", "client_credentials");
        dict.Add("scope", "viewables:read");
        var response =
            await new HttpClient()
                .PostAsync("https://developer.api.autodesk.com/authentication/v2/token",
                    new FormUrlEncodedContent(dict));
        // var token = await response.Content.re<TokenResponse>();

        // return token.access_token;
        
        return string.Empty;
    }
}

public class TokenResponse
{
    public string access_token;
}