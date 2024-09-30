namespace ViewerV2.Services;

public interface IForgeTokenService
{
    Task<string?> GetForgeToken();
}