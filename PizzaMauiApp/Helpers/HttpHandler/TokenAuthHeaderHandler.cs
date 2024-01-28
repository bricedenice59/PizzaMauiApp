using System.Net.Http.Headers;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.Helpers.HttpHandler;

public class TokenAuthHeaderHandler: DelegatingHandler
{
    private readonly ITokenService _tokenService;
    private readonly ILogger _logger;
    
    public TokenAuthHeaderHandler(ITokenService tokenService, ILogger logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => await MakeRequest(request, false, cancellationToken);


    private async Task<HttpResponseMessage> MakeRequest(HttpRequestMessage request, bool retry, CancellationToken cancellationToken)
    {
        var token = await _tokenService.GetAccessTokenAsync();
        var refreshToken = await _tokenService.GetRefreshTokenAsync();
        
        if(!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await base.SendAsync(request, cancellationToken);
        
        if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized || retry) return response;
        
        _logger.Information("User token may have expired... try to get a new one.");

        var hasSucceeded = await _tokenService.RefreshTokensAsync(token, refreshToken);
        if(hasSucceeded)
            return await MakeRequest(request, true, cancellationToken);

        throw new HttpRequestException("Could not refresh authentication token.");
    }
}
