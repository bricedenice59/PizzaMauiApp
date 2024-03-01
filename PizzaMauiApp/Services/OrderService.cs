using PizzaMauiApp.API.Dtos;

namespace PizzaMauiApp.Services;

public interface IOrderService
{
    Task<bool> CreateOrderAsync(OrderDto basketDto, CancellationToken cancellationToken = new());
}

public class OrderService : IOrderService
{
    private readonly IRequestApiService _requestApiService;
    private readonly IAppSettings _appSettings;
    private readonly ILogger _logger;
    
    public OrderService(ILogger logger, 
        IRequestApiService requestApiService, 
        IAppSettings appSettings)
    {
        _logger = logger;
        _requestApiService = requestApiService;
        _appSettings = appSettings;
    }

    public async Task<bool> CreateOrderAsync(OrderDto orderDto, CancellationToken cancellationToken = new())
    {
        _logger.Information("Try to create order...");
        string endPoint = _appSettings.Settings.OrderAPI.EndpointUrl + _appSettings.Settings.OrderAPI.CreateEndPointName;
        _logger.Information($"Request API {endPoint}");

        var createOrderResponse = 
            await _requestApiService.Post<OrderDto>(
                endPoint,
                orderDto,
                cancellationToken);
        
        _logger.Information(createOrderResponse ? "Order created successfully..." : "Order creation failed...");
        return createOrderResponse;
    }
}