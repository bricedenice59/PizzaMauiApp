using PizzaMauiApp.API.Dtos;

namespace PizzaMauiApp.Services;

public interface ILoginSignupService
{
    Task<(bool, string?)> LoginUserAsync(string email, string password, CancellationToken cancellationToken = new());
    Task<(bool, string?)> RegisterUserAsync(string email, string password, CancellationToken cancellationToken = new());
}

public class LoginSignupService : ILoginSignupService
{
    private readonly IRequestApiService _requestApiService;
    private readonly IAppSettings _appSettings;
    private readonly ILogger _logger;
    
    public LoginSignupService(ILogger logger, 
        IRequestApiService requestApiService, 
        IAppSettings appSettings)
    {
        _logger = logger;
        _requestApiService = requestApiService;
        _appSettings = appSettings;
    }

    public async Task<(bool, string?)> LoginUserAsync(string email, string password, CancellationToken cancellationToken = new())
    {
        _logger.Information("Try login user...");
        string endPoint = _appSettings.Settings.WebAPI.EndpointUrl + _appSettings.Settings.WebAPI.LoginEndpointName;
        _logger.Information($"Request API {endPoint}");

        var userIdentityResponse = 
            await _requestApiService.Post<UserLoginDto, UserIdentityDto>(
                endPoint,
                new UserLoginDto { Email = email, Password = password },
                cancellationToken);
        
        _logger.Information(userIdentityResponse == null ? "Login failed..." : "Login succeeded...");
        return (userIdentityResponse != null, userIdentityResponse?.Token);
    }

    public async Task<(bool, string?)> RegisterUserAsync(string email, string password, CancellationToken cancellationToken = new())
    {
        _logger.Information("Try register user...");
        string endPoint = _appSettings.Settings.WebAPI.EndpointUrl + _appSettings.Settings.WebAPI.RegisterEndpointName;
        _logger.Information($"Request API {endPoint}");

        var userIdentityResponse = 
            await _requestApiService.Post<UserRegisterDto, UserIdentityDto>(
                endPoint,
                new UserRegisterDto { Email = email, Password = password },
                cancellationToken);

        _logger.Information(userIdentityResponse == null ? "Register user failed..." : "Register user succeeded...");
        return (userIdentityResponse != null, userIdentityResponse?.Token);
    }
}