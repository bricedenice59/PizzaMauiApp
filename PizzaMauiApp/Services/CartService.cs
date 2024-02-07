
using PizzaMauiApp.API.Dtos;
using PizzaMauiApp.ViewModels.Models;

namespace PizzaMauiApp.Services;

public interface ICartService
{
    Task<bool> AddOneToCart(Guid pizzaId, CancellationToken cancellationToken = new());
    Task<bool> ClearCart(CancellationToken cancellationToken = new());
    Task<Dictionary<Guid, int>> GetAllFromCart(CancellationToken cancellationToken = new());
    Task<bool> RemoveOneFromCart(Guid pizzaId, CancellationToken cancellationToken = new());
    Task<bool> RemoveAllFromCart(Guid pizzaId, CancellationToken cancellationToken = new());
}

public class CartService : ICartService
{
    private readonly IRequestApiService _requestApiService;
    private readonly ILogger _logger;

    private readonly string _cartEndPoint;
    
    public CartService(ILogger logger, 
            IRequestApiService requestApiService,
            IAppSettings appSettings)
    {
        _logger = logger;
        _requestApiService = requestApiService;
        if (!Preferences.ContainsKey(PreferencesStorageModel.UserId) &&
            !string.IsNullOrEmpty(Preferences.Get(PreferencesStorageModel.UserId, string.Empty)))
            throw new ArgumentNullException("A user id must be saved in order to get access to the cart service!");
        
        _cartEndPoint = appSettings.Settings.WebAPI.EndpointUrl + appSettings.Settings.WebAPI.CartEndpoint;
    }
    
    #region Private methods

    private async Task<bool> UpdateCart(Guid pizzaId, int quantity, CancellationToken cancellationToken = new())
    {
        #region Get user current cart

        CustomerBasketDto? customerBasketItemDtoResponse = await GetCustomerCart(cancellationToken);

        if (customerBasketItemDtoResponse == null)
        {
            _logger.Information("Could not get current user cart.");
            return false;
        }

        #endregion
        
        #region Remove items from cart if quantity == 0

        if (quantity == 0)
        {
            var pizzaItem = customerBasketItemDtoResponse.Items.FirstOrDefault(x=>x.Id == pizzaId);
            if (pizzaItem == null) return false;
            
            pizzaItem.Quantity = 0;
            customerBasketItemDtoResponse.Items.Remove(pizzaItem);
            return await UpdateCart(customerBasketItemDtoResponse, cancellationToken);
        }
        
        #endregion
        
        #region Add one or remove one item from cart
        
        _logger.Information($"Try adding/removing pizza with id:{pizzaId} to user cart...");

        if (!customerBasketItemDtoResponse.Items.Any(x => x.Id == pizzaId))
        {
            customerBasketItemDtoResponse.Items.Add(new BasketItemDto { Id = pizzaId, Quantity = quantity });
        }
        else
        { 
            var pizzaItem = customerBasketItemDtoResponse.Items.FirstOrDefault(x=>x.Id == pizzaId);
            if (pizzaItem == null) return false;
            
            if (pizzaItem.Quantity + quantity < 0)
            {
                return false;
            }
            pizzaItem.Quantity += quantity;
        }

        return await UpdateCart(customerBasketItemDtoResponse, cancellationToken);
        #endregion
    }
    
    private async Task<bool> UpdateCart(CustomerBasketDto customerBasketDto, CancellationToken cancellationToken = new())
    {
        var hasUpdateSucceeded = 
            await _requestApiService.Post<CustomerBasketDto, bool>(
                _cartEndPoint,
                customerBasketDto,
                cancellationToken);
        
        _logger.Information(hasUpdateSucceeded? "Cart updated successfully..." : "Cart update has failed.");
        return hasUpdateSucceeded;
    }

    private async Task<CustomerBasketDto?> GetCustomerCart(CancellationToken cancellationToken = new())
    {
        _logger.Information("Try getting current user cart...");
        _logger.Information($"Request API {_cartEndPoint}");
        
        return  
            await _requestApiService.Get<CustomerBasketDto>(
                _cartEndPoint+ $"?customerId={Preferences.Get(PreferencesStorageModel.UserId, string.Empty)}",
                cancellationToken);
    }
    
    #endregion

    #region ICartService implementation
    
    public async Task<bool> ClearCart(CancellationToken cancellationToken = new())
    {
        _logger.Information("Try deleting current user cart...");
        _logger.Information($"Request API {_cartEndPoint}");
        var clearCartSucceeded =   
            await _requestApiService.Delete<bool>(
                _cartEndPoint+ $"?customerId={Preferences.Get(PreferencesStorageModel.UserId, string.Empty)}",
                cancellationToken);
        
        _logger.Information(clearCartSucceeded? "Cart deleted successfully..." : "Cart deletion has failed.");
        return clearCartSucceeded;
    }
    
    public async Task<Dictionary<Guid, int>> GetAllFromCart(CancellationToken cancellationToken = new())
    {
        CustomerBasketDto? customerBasketItemDtoResponse = await GetCustomerCart(cancellationToken);

        if (customerBasketItemDtoResponse == null)
        {
            _logger.Information("Could not get current user cart.");
            return new Dictionary<Guid, int>();
        }

        return customerBasketItemDtoResponse.Items.ToDictionary(x=>x.Id,x=>x.Quantity);
    }
    
    public async Task<bool> AddOneToCart(Guid pizzaId, CancellationToken cancellationToken = new())
    {
        return await UpdateCart(pizzaId, 1, cancellationToken);
    }
    
    public async Task<bool> RemoveOneFromCart(Guid pizzaId, CancellationToken cancellationToken = new())
    {
        return await UpdateCart(pizzaId, -1, cancellationToken);
    }
    
    public async Task<bool> RemoveAllFromCart(Guid pizzaId, CancellationToken cancellationToken = new())
    {
        return await UpdateCart(pizzaId, 0, cancellationToken);
    }
    
    #endregion
}