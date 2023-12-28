using PizzaMauiApp.Models;

namespace PizzaMauiApp.Services;

public class PizzaService : IPizzaService
{
    private const string AllPizzas = "All";
    private const string PopularPizzas = "Popular";
    
    private readonly IEnumerable<Pizza> _pizzas = new[]
    {
        new Pizza
        {
            Name = "Anchovies",
            Price = 15.99,
            Image = "anchovies.png",
            Ingredients = "Cheese, Tomato sauce, anchovies",
            Description = "Enjoy a crispy thin crust pizza topped with tangy tomato sauce, gooey mozzarella cheese, and salty anchovies for a burst of flavour in every bite."
        },
        new Pizza
        {
            Name = "BBQ",
            Price = 14.50,
            Image = "barbecue.png",
            Ingredients = "Cheese, Tomato sauce, Chicken,Corn, Red Onions",
            Description = "Sink your teeth into a delicious barbecue pizza with smoky chicken, sweet corn, red onion, and melted cheddar cheese on a soft and chewy crust."
        },
        new Pizza
        {
            Name = "Calzone",
            Price = 14,
            Image = "calzone.png",
            Ingredients = "Cheese, Tomato sauce, Ham, Pepperoni, Spinach",
            Description = "Indulge in a warm and cheesy calzone filled with spicy pepperoni, fresh spinach, ricotta cheese, and marinara sauce, wrapped in a golden-brown dough."
        },
        new Pizza
        {
            Name = "4 Cheeses",
            Price = 17.99,
            Image = "cheese4.png",
            Ingredients = "Mozzarella, Parmesan, Gorgonzola, Provolone cheese, Cream sauce",
            Description = "Treat yourself to a pizza 4 cheese with a rich and creamy blend of mozzarella, parmesan, gorgonzola, and provolone cheese on a crispy crust, sprinkled with fresh basil and oregano."
        },
        new Pizza
        {
            Name = "Green Pesto",
            Price = 12.70,
            Image = "greenpesto.png",
            Ingredients = "Cheese, Tomato sauce, Green Basil, Olives",
            Description = "Savour a pizza green pesto with a fragrant and nutty sauce made from basil, pine nuts, garlic, and olive oil, topped with sliced tomatoes, mozzarella cheese, and black olives."
        },
        new Pizza
        {
            Name = "Hawaiian",
            Price = 13,
            Image = "hawaiian.png",
            Ingredients = "Cheese, Tomato sauce, Ham, Pineapple",
            Description = "Experience a tropical taste with a pizza hawaiian style, featuring juicy pineapple chunks, tender ham, and melted mozzarella cheese on a tangy tomato sauce and a fluffy crust."
        },
        new Pizza
        {
            Name = "Italian",
            Price = 17.99,
            Image = "italianham.png",
            Ingredients = "Cheese, Tomato sauce, Italian smoked Ham, Basil",
            Description = "The classical Italian pizza is a simple and authentic dish, with crispy dough, fresh tomato sauce, fragrant basil, and soft mozzarella cheese."
        },
        new Pizza
        {
            Name = "Black Olives",
            Price = 11.50,
            Image = "olives.png",
            Ingredients = "Cheese, Tomato sauce, Black olives, Olive oil",
            Description = "The pizza black olives is a Mediterranean delight, with creamy cheese, garlicky tomato sauce, and briny black olives on a chewy crust."
        },
        new Pizza
        {
            Name = "Pepperoni",
            Price = 14,
            Image = "pepperoni.png",
            Ingredients = "Cheese, Tomato sauce, Pepperoni, Olive oil",
            Description = "The pizza pepperoni is a meaty delight, with stretchy cheese, rich tomato sauce, and spicy pepperoni slices on a golden crust."
        },
        new Pizza
        {
            Name = "Salmon",
            Price = 18.50,
            Image = "salmon.png",
            Ingredients = "Cheese, Tomato sauce, Salmon slices.",
            Description = "The pizza salmon is a scrumptious delight, with creamy cheese, tangy tomato sauce, and succulent salmon slices on a crispy crust."
        },
        new Pizza
        {
            Name = "Tuna",
            Price = 14.99,
            Image = "tuna.png",
            Ingredients = "Cheese, Tomato sauce, Tuna chunks.",
            Description = "The pizza tuna is a savory treat, with melted cheese, spicy tomato sauce, and flaky tuna chunks on a thin crust."
        },        
        new Pizza
        {
            Name = "Veggie",
            Price = 11.99,
            Image = "veggie.png",
            Ingredients = "Cheese, Tomato sauce, Peppers, Mushrooms, Onions, Olives, Spinach, Broccoli, Artichokes",
            Description = "The pizza veggie is a colorful feast, with gooey cheese, fresh tomato sauce, and crunchy vegetables on a fluffy crust."
        }
    };

    public Task<IEnumerable<Pizza>> GetAll()
    {
        if (Cache.Any(x => x.Item1 == AllPizzas))
            return Task.FromResult(Cache.First(x => x.Item1 == AllPizzas).Item2);
        
        var all = Task.FromResult(_pizzas);
        Cache.Add(new ValueTuple<string, IEnumerable<Pizza>>(AllPizzas, _pizzas));
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