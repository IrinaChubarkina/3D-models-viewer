namespace Viewer.Services;

public interface IForgeTokenService
{
    Task<string?> GetForgeToken();
}