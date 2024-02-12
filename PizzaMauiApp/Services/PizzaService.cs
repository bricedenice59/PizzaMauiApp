using System.Collections.Concurrent;
using PizzaMauiApp.API.Dtos;
using PizzaMauiApp.Models;
namespace PizzaMauiApp.Services;

public class PizzaService : IPizzaService
{
    private const string AllPizzas = "All";
    private const string PopularPizzas = "Popular";
    
    private readonly IRequestApiService _requestApiService;
    private readonly IAppSettings _appSettings;
    private readonly ILogger _logger;
    public PizzaService(ILogger logger, 
        IRequestApiService requestApiService, 
        IAppSettings appSettings)
    {
        _logger = logger;
        _requestApiService = requestApiService;
        _appSettings = appSettings;
    }

    public async Task<Pizza?> GetById(Guid id)
    {
        if (id.Equals(Guid.Empty))
            return null;
        return await Task.Run(async () =>
        {
            var allPizzas = await GetAll();
            return allPizzas?.FirstOrDefault(x => x.Id == id);
        });
    }

    public async ValueTask<IEnumerable<Pizza>?> GetAll(CancellationToken cancellationToken = new ())
    {
        if (Cache.Any(x => x.Key == AllPizzas))
        {
            _logger.Information($"GetAll from cache.");
            return Cache[AllPizzas];
        }
 
        string endPoint = _appSettings.Settings.WebAPI.EndpointUrl + _appSettings.Settings.WebAPI.GetAllEndpointName;
        _logger.Information($"Request API {endPoint}");
        
        var pizzas = await _requestApiService.Get<IEnumerable<PizzaProductDto>>(endPoint, cancellationToken);
        if (pizzas == null ) 
            return null;

        List<Pizza> lstPizza = [];
        lstPizza.AddRange(pizzas.Select(pizzaProduct => new Pizza
        {
            Id = new Guid(pizzaProduct.Id),
            Name = pizzaProduct.Name,
            Description = pizzaProduct.Description,
            Ingredients = pizzaProduct.Ingredients,
            MainImageUrl = pizzaProduct.MainImageUrl,
            PriceWithExcludedVAT = pizzaProduct.PriceWithExcludedVAT
        }));
        
        return Cache.TryAdd(AllPizzas, lstPizza) ? Cache[AllPizzas] : lstPizza;
    }

    public async Task<IEnumerable<Pizza>?> Lookup(string key)
    {
        var allPizzas = await GetAll();

        if (allPizzas != null)
            return string.IsNullOrEmpty(key)
                ? allPizzas
                : allPizzas.Where(x => x.Name.Contains(key, StringComparison.OrdinalIgnoreCase));

        return null;
    }

    public async ValueTask<IEnumerable<Pizza>?> GetPopular(int count = 6)
    {
        if (Cache.Any(x => x.Key == PopularPizzas))
            return Cache[PopularPizzas];
        
        var allPizzas = await GetAll();
        if (allPizzas == null)
            return allPizzas;
        
        var popular = allPizzas.OrderBy(x => Guid.NewGuid())
            .Take(count)
            .ToList();
        
        return Cache.TryAdd(PopularPizzas, popular) ? Cache[PopularPizzas] : popular;
    }

    public ConcurrentDictionary<string, IEnumerable<Pizza>?> Cache { get; } = new();
}

public interface IPizzaService
{
    Task<Pizza?> GetById(Guid id);
    ValueTask<IEnumerable<Pizza>?> GetAll(CancellationToken cancellationToken = new());
    Task<IEnumerable<Pizza>?> Lookup(string key);
    ValueTask<IEnumerable<Pizza>?> GetPopular(int count = 6);
}