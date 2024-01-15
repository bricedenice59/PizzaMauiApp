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

    public Task<Pizza?> GetById(Guid id)
    {
        if (id.Equals(Guid.Empty))
            return Task.FromResult<Pizza?>(null);
        return Task.Run(async () =>
        {
            var allPizzas = await GetAll();
            return allPizzas.FirstOrDefault(x => x.Id == id);
        });
    }

    public async Task<IEnumerable<Pizza>> GetAll(CancellationToken cancellationToken = new ())
    {
        if (Cache.Any(x => x.Item1 == AllPizzas))
        {
            _logger.Information($"GetAll from cache.");
            return await Task.FromResult(Cache.First(x => x.Item1 == AllPizzas).Item2);
        }
 
        string endPoint = _appSettings.Settings.WebAPI.EndpointUrl + _appSettings.Settings.WebAPI.GetAllEndpointName;
        _logger.Information($"Request API {endPoint}");
        
        var all = await _requestApiService.Get<IEnumerable<Pizza>>(endPoint, cancellationToken);
        if (all.Item2 == null) return new List<Pizza>();
        
        Cache.Add(new ValueTuple<string, IEnumerable<Pizza>>(AllPizzas, all.Item2));
        return all.Item2;
    }

    public async Task<IEnumerable<Pizza>> Lookup(string key)
    {
        var allPizzas = await GetAll();
            
        return string.IsNullOrEmpty(key)
            ? allPizzas
            : allPizzas.Where(x => x.Name.Contains(key, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<Pizza>> GetPopular(int count = 6)
    {
        if (Cache.Any(x => x.Item1 == PopularPizzas))
            return Cache.First(x => x.Item1 == PopularPizzas).Item2;
        
        var allPizzas = await GetAll();
        if (!allPizzas.Any())
            return allPizzas;
        
        var popular = allPizzas.OrderBy(x => Guid.NewGuid())
            .Take(count);
        Cache.Add(new ValueTuple<string, IEnumerable<Pizza>>(PopularPizzas, popular));

        return popular;
    }

    public List<(string, IEnumerable<Pizza>)> Cache { get; } = new();
}

public interface IPizzaService
{
    Task<Pizza?> GetById(Guid id);
    Task<IEnumerable<Pizza>> GetAll(CancellationToken cancellationToken = new());
    Task<IEnumerable<Pizza>> Lookup(string key);
    Task<IEnumerable<Pizza>> GetPopular(int count = 6);
}