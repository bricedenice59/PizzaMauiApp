namespace PizzaMauiApp.ViewModels.Models;

public class CartPizzaModel
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Image { get; set; }
    public required double Price { get; set; }
    public required int Quantity { get; set; }
    public required double Amount { get; set; }
}