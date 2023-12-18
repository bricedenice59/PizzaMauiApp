using CommunityToolkit.Mvvm.ComponentModel;

namespace PizzaMauiApp.Models;

public partial class Pizza : ObservableObject
{
    public required string Name { get; set; }
    public required string Image { get; set; }
    public required double Price { get; set; }

    [ObservableProperty, NotifyPropertyChangedFor(nameof(Amount))]
    private int _quantity = 0;

    public double Amount => Quantity * Price;

}