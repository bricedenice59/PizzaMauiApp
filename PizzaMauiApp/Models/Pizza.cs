namespace PizzaMauiApp.Models;

public partial class Pizza : ObservableObject
{
    public required string Name { get; set; }
    public required string Image { get; set; }
    public required double Price { get; set; }
    
    public required string Description { get; set; }
    
    public required string Ingredients { get; set; }

    [ObservableProperty, NotifyPropertyChangedFor(nameof(Amount))]
    private int _quantity = 0;

    public double Amount => Quantity * Price;

}