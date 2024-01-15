using System.Net.Http.Headers;
using System.Text.Json;

namespace PizzaMauiApp.Services;

public interface IRequestApiService
{
    Task<(int,T?)> Get<T>(string endpointUrl, CancellationToken cancellationToken, TimeSpan timeout = default);
}

public class RequestApiService : IRequestApiService
{
    private readonly ILogger _logger;
    private readonly TimeSpan _duration = TimeSpan.FromMilliseconds(20000); //20s
    
    public RequestApiService(ILogger logger)
    {
        _logger = logger;
    }
    
    public async Task<(int,T?)> Get<T>(string endpointUrl, CancellationToken cancellationToken, TimeSpan timeout = default)
    {
        if (cancellationToken.IsCancellationRequested)
            return (400, default);

        var tsTimeout = 
            timeout == default 
                ? _duration 
                : timeout;
        try
        {
            using var client = new HttpClient();
            client.Timeout = tsTimeout;
            client.BaseAddress = new Uri(endpointUrl);
            
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        
            // Get data response
            var response = await client.GetAsync(endpointUrl, cancellationToken); 
            var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            
            // Deserialize the JSON response
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var responseData = await JsonSerializer.DeserializeAsync<T>(responseStream, options, cancellationToken: cancellationToken);
            return ((int)response.StatusCode, responseData);
        }
        catch (TaskCanceledException tce)
        {
            _logger.Warning(tce, "The request timed out");
            // The request timed out (error 408)
            return (408, default);
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, ex.Message);
            return (400, default);
        }
    }
}