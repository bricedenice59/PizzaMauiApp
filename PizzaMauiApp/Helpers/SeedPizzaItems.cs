using System.Text.Json;
using PizzaMauiApp.Models;

namespace PizzaMauiApp.Helpers;

public class SeedPizzaItems
{
    public static async Task<IEnumerable<Pizza>?> GetItems()
    {
        await using var stream = await FileSystem.OpenAppPackageFileAsync("items.json");
        return await JsonSerializer.DeserializeAsync<List<Pizza>>(stream);
    }
}