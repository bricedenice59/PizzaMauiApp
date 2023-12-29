using System.Text.Json;
using PizzaMauiApp.Helpers;
using PizzaMauiApp.Models;

namespace PizzaMauiApp.Services;

public class PizzaService : IPizzaService
{
    private const string AllPizzas = "All";
    private const string PopularPizzas = "Popular";
    
    public async Task<IEnumerable<Pizza>> GetAll()
    {
        if (Cache.Any(x => x.Item1 == AllPizzas))
            return await Task.FromResult(Cache.First(x => x.Item1 == AllPizzas).Item2);
        
        var all = await SeedPizzaItems.GetItems();
        Cache.Add(new ValueTuple<string, IEnumerable<Pizza>>(AllPizzas, all));
        return all;
    }

    public Task<IEnumerable<Pizza>> Lookup(string key)
    {
        return Task.Run(async () =>
        {
            var allPizzas = await GetAll();
            
            return string.IsNullOrEmpty(key)
                ? allPizzas
                : allPizzas.Where(x => x.Name.Contains(key, StringComparison.OrdinalIgnoreCase));
        });
    }
      

    public Task<IEnumerable<Pizza>> GetPopular(int count = 6)
    {
        return Task.Run( async () =>
        {
            if (Cache.Any(x => x.Item1 == PopularPizzas))
                return Cache.First(x => x.Item1 == PopularPizzas).Item2;
            
            var allPizzas = await GetAll();
            
            var popular = allPizzas.OrderBy(x => Guid.NewGuid())
                .Take(count);
            Cache.Add(new ValueTuple<string, IEnumerable<Pizza>>(PopularPizzas, popular));

            return popular;
        });
    }

    public List<(string, IEnumerable<Pizza>)> Cache { get; } = new();
}

public interface IPizzaService
{
    Task<IEnumerable<Pizza>> GetAll();
    Task<IEnumerable<Pizza>> Lookup(string key);
    Task<IEnumerable<Pizza>> GetPopular(int count = 6);
}