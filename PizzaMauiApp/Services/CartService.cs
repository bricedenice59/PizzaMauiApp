
namespace PizzaMauiApp.Services;

public interface ICartService
{
    void AddToCart(Guid pizzaId);
    Dictionary<Guid, int> GetCart();
    void RemoveFromCart(Guid pizzaId);
}

public class CartService : ICartService
{
    private Dictionary<Guid, int> _cartItems { get; } = new();

    public void AddToCart(Guid pizzaId)
    {
        if (!_cartItems.ContainsKey(pizzaId))
            _cartItems.Add(pizzaId, 1);
        else _cartItems[pizzaId] += 1;
    }

    public Dictionary<Guid, int> GetCart() => _cartItems;
    
    public void RemoveFromCart(Guid pizzaId)
    {
        if (!_cartItems.ContainsKey(pizzaId))
            return;
        var quantity = _cartItems[pizzaId];
        if (quantity - 1 == 0)
        {
            _cartItems.Remove(pizzaId);
            return;
        }
        
        _cartItems[pizzaId] -= 1;
    }
}