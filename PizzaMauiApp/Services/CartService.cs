
namespace PizzaMauiApp.Services;

public interface ICartService
{
    void AddToCart(Guid pizzaId);
    void ClearCart();
    Dictionary<Guid, int> GetCart();
    void RemoveOneFromCart(Guid pizzaId);
    void RemoveAllFromCart(Guid pizzaId);
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

    public void ClearCart()
    {
        _cartItems.Clear();
    }

    public Dictionary<Guid, int> GetCart() => _cartItems;
    
    public void RemoveOneFromCart(Guid pizzaId)
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
    
    public void RemoveAllFromCart(Guid pizzaId)
    {
        if (!_cartItems.ContainsKey(pizzaId))
            return;
        _cartItems.Remove(pizzaId);
    }
}