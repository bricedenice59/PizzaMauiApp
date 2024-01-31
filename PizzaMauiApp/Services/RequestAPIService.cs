using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using PizzaMauiApp.Helpers.API;

namespace PizzaMauiApp.Services;

public interface IRequestApiService
{
    Task<T?> Delete<T>(string endpointUrl, CancellationToken cancellationToken =  new (), TimeSpan timeout = default);
    Task<T?> Get<T>(string endpointUrl, CancellationToken cancellationToken =  new (), TimeSpan timeout = default);
    Task<TOut?> Post<TIn, TOut>(string endpointUrl, TIn inObject, CancellationToken cancellationToken = new (), TimeSpan timeout = default);
}

public class RequestApiService : IRequestApiService
{
    private readonly ILogger _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TimeSpan _timeoutDuration = TimeSpan.FromMilliseconds(20000); //20s
    
    public RequestApiService(ILogger logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<T?> Delete<T>(string endpointUrl, CancellationToken cancellationToken, TimeSpan timeout = default)
    {
        if (cancellationToken.IsCancellationRequested)
            return default;

        var tsTimeout =
            timeout == default
                ? _timeoutDuration
                : timeout;
        try
        {
            using var client = _httpClientFactory.CreateClient("customHttpClient");
            client.Timeout = tsTimeout;
            client.BaseAddress = new Uri(endpointUrl);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Get data response
            var response = await client.DeleteAsync(endpointUrl, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.Warning(
                    $"The request failed with statusCode: {(int)response.StatusCode}, reason: {response.ReasonPhrase}");
                return default;
            }

            var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);

            // Deserialize the JSON response
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var responseData = await JsonSerializer.DeserializeAsync<ApiResponse<T>>(responseStream, options,
                cancellationToken: cancellationToken);
            if (responseData != null && !responseData.Success)
            {
                _logger.Warning(
                    $"The request failed with statusCode: {(int)responseData.StatusCode}, reason: {responseData.Message}");
                return default;
            }

            return responseData != null ? responseData.Data : default;
        }
        catch (TaskCanceledException tce)
        {
            _logger.Warning(tce, "The request timed out");
            // The request timed out (error 408)
            return default;
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, ex.Message);
            return default;
        }
    }

    public async Task<T?> Get<T>(string endpointUrl, CancellationToken cancellationToken, TimeSpan timeout = default)
    {
        if (cancellationToken.IsCancellationRequested)
            return default;

        var tsTimeout = 
            timeout == default 
                ? _timeoutDuration 
                : timeout;
        try
        {
            using var client = _httpClientFactory.CreateClient("customHttpClient");
            client.Timeout = tsTimeout;
            client.BaseAddress = new Uri(endpointUrl);
            
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // Get data response
            var response = await client.GetAsync(endpointUrl, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.Warning( $"The request failed with statusCode: {(int)response.StatusCode}, reason: {response.ReasonPhrase}");
                return default;
            }
            var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            
            // Deserialize the JSON response
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var responseData = await JsonSerializer.DeserializeAsync<ApiResponse<T>>(responseStream, options, cancellationToken: cancellationToken);
            if (responseData != null && !responseData.Success)
            {
                _logger.Warning( $"The request failed with statusCode: {(int)responseData.StatusCode}, reason: {responseData.Message}");
                return default;
            }
            return responseData != null ? responseData.Data : default;
        }
        catch (TaskCanceledException tce)
        {
            _logger.Warning(tce, "The request timed out");
            // The request timed out (error 408)
            return default;
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, ex.Message);
            return default;
        }
    }

    public async Task<TOut?> Post<TIn, TOut>(string endpointUrl, TIn inObject, CancellationToken cancellationToken,
        TimeSpan timeout = default)
    {
        if (cancellationToken.IsCancellationRequested)
            return default;

        var tsTimeout = 
            timeout == default 
                ? _timeoutDuration 
                : timeout;
        try
        {
            using var client = _httpClientFactory.CreateClient("customHttpClient");
            client.Timeout = tsTimeout;
            client.BaseAddress = new Uri(endpointUrl);
            
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        
            // Get data response
            var response = await client.PostAsJsonAsync(endpointUrl, inObject, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.Warning( $"The request failed with statusCode: {(int)response.StatusCode}, reason: {response.ReasonPhrase}");
                return default;
            }
            var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            
            // Deserialize the JSON response
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var responseData = await JsonSerializer.DeserializeAsync<ApiResponse<TOut>>(responseStream, options, cancellationToken: cancellationToken);
            if (responseData != null && !responseData.Success)
            {
                _logger.Warning( $"The request failed with statusCode: {(int)responseData.StatusCode}, reason: {responseData.Message}");
                return default;
            }

            return responseData != null ? responseData.Data : default;
        }
        catch (TaskCanceledException tce)
        {
            _logger.Warning(tce, "The request timed out");
            // The request timed out (error 408)
            return default;
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, ex.Message);
            return default;
        }
    }
}