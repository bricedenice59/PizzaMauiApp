using PizzaMauiApp.Models;

namespace PizzaMauiApp.ViewModels;

public partial class CartViewModel : ViewModelBase
{
    public ObservableCollection<Pizza> Items { get; set; } = new();

    [ObservableProperty] private double _totalAmount;

    [RelayCommand]
    private async Task UpdateCart(Pizza pizzaItem)
    {
        var item = Items.FirstOrDefault(i => i.Id == pizzaItem.Id);
        if (item is not null)
            item.Quantity = pizzaItem.Quantity;
        else
        {
        }
    }
}