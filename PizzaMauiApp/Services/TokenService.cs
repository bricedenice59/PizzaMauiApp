using PizzaMauiApp.API.Dtos;
using PizzaMauiApp.ViewModels.Models;

namespace PizzaMauiApp.Services;

public interface ITokenService
{
    Task<string> GetRefreshTokenAsync();
    Task<string> GetAccessTokenAsync();
    Task<bool> RefreshTokensAsync(string userToken, string userRefreshToken ,CancellationToken cancellationToken = new());
}

public class TokenService : ITokenService
{
    private readonly IRequestApiService _requestApiService;
    private readonly ILogger _logger;
    private readonly IAppSettings _appSettings;
    
    public TokenService(
        ILogger logger, 
        IRequestApiService requestApiService,
        IAppSettings appSettings)
    {
        _logger = logger;
        _requestApiService = requestApiService;
        _appSettings = appSettings;
    }
    
    public Task<string> GetAccessTokenAsync()
    {
        return Task.FromResult(Preferences.Get(PreferencesStorageModel.UserToken, ""));
    }
    
    public Task<string> GetRefreshTokenAsync()
    {
        return Task.FromResult(Preferences.Get(PreferencesStorageModel.UserRefreshToken, ""));
    }

    public async Task<bool> RefreshTokensAsync(string userToken, string userRefreshToken, CancellationToken cancellationToken = new())
    {
        _logger.Information("Try to refresh user token...");
        string endPoint = _appSettings.Settings.WebAPI.EndpointUrl + _appSettings.Settings.WebAPI.RefreshTokenEndpointName;
        _logger.Information($"Request API {endPoint}");

        var response = await _requestApiService.Post<TokenModelDto, UserIdentityDto>(
            endPoint,
            new TokenModelDto { AccessToken = userToken, RefreshToken = userRefreshToken },
            cancellationToken);

        _logger.Information(response != null ? "User token has been refreshed successfully." : "There was an issue trying to refresh user token");

        var previousToken = await GetAccessTokenAsync();
        var previousRefreshToken = await GetRefreshTokenAsync();
        var hasSucceeded = !string.IsNullOrEmpty(previousToken) &&
                           !string.IsNullOrEmpty(response?.Token) &&
                           response.Token != previousToken &&
                           !string.IsNullOrEmpty(previousRefreshToken) &&
                           !string.IsNullOrEmpty(response?.RefreshToken) &&
                           response.RefreshToken != previousRefreshToken;
        if (hasSucceeded)
        {
            Preferences.Set(PreferencesStorageModel.UserToken, response!.Token);
            Preferences.Set(PreferencesStorageModel.UserRefreshToken, response!.RefreshToken);
        }

        return hasSucceeded;
    }
}