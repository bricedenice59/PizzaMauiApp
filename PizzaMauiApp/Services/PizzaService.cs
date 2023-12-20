using PizzaMauiApp.Models;

namespace PizzaMauiApp.Services;

public class PizzaService : IPizzaService
{
    private readonly IEnumerable<Pizza> _pizzas = new[]
    {
        new Pizza
        {
            Name = "Anchovies",
            Price = 15.99,
            Image = "anchovies.png"
        },
        new Pizza
        {
            Name = "BBQ",
            Price = 14.50,
            Image = "barbecue.png"
        },
        new Pizza
        {
            Name = "Calzone",
            Price = 14,
            Image = "calzone.png"
        },
        new Pizza
        {
            Name = "4 Cheeses",
            Price = 17.99,
            Image = "cheese4.png"
        },
        new Pizza
        {
            Name = "Green Pesto",
            Price = 12.70,
            Image = "greenpesto.png"
        },
        new Pizza
        {
            Name = "Hawaiian",
            Price = 13,
            Image = "hawaiian.png"
        },
        new Pizza
        {
            Name = "Italian",
            Price = 17.99,
            Image = "italianham.png"
        },
        new Pizza
        {
            Name = "Black Olives",
            Price = 11.50,
            Image = "olives.png"
        },
        new Pizza
        {
            Name = "Pepperoni",
            Price = 14,
            Image = "pepperoni.png"
        },
        new Pizza
        {
            Name = "Salmon",
            Price = 18.50,
            Image = "salmon.png"
        },
        new Pizza
        {
            Name = "Tuna",
            Price = 14.99,
            Image = "tuna.png"
        },        
        new Pizza
        {
            Name = "Veggie",
            Price = 11.99,
            Image = "veggie.png"
        }
    };

    public Task<IEnumerable<Pizza>> GetAll()
    {
        return Task.FromResult(_pizzas);
    }

    public  Task<IEnumerable<Pizza>> Lookup(string key)
    {
        return Task.Run(() =>
        {
            return string.IsNullOrEmpty(key)
                ? _pizzas
                : _pizzas.Where(x => x.Name.Contains(key, StringComparison.OrdinalIgnoreCase));
        });
    }
      

    public Task<IEnumerable<Pizza>> GetPopular(int count = 6)
    {
        return Task.Run( () =>
        {
            return _pizzas.OrderBy(x => Guid.NewGuid())
                .Take(count);
        });
    }
}

public interface IPizzaService
{
    Task<IEnumerable<Pizza>> GetAll();
    Task<IEnumerable<Pizza>> Lookup(string key);
    Task<IEnumerable<Pizza>> GetPopular(int count = 6);
}