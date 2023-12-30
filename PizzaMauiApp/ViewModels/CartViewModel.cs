using PizzaMauiApp.Models;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class CartViewModel(IDialogService dialogService, IToastService toastService) : ViewModelBase
{
    public ObservableCollection<Pizza> Items { get; set; } = new();

    [ObservableProperty] private double _totalAmount;

    [RelayCommand]
    private void OnUpdateCart(Pizza pizzaItem)
    {
        var item = Items.FirstOrDefault(i => i.Id == pizzaItem.Id);
        if (item is not null)
            item.Quantity = pizzaItem.Quantity;
        else
        {

        }
        RecalculateTotalAmount();
    }

    [RelayCommand]
    private void OnRemoveFromCart(Guid pizzaItemId)
    {
        var item = Items.FirstOrDefault(i => i.Id == pizzaItemId);
        if (item != null)
        {
            Items.Remove(item);
            RecalculateTotalAmount();
        }
    }

    [RelayCommand]
    private async Task OnClearCart()
    {
        if (await dialogService.DisplayConfirm("Confirm clear cart?", "Do you really want to clear the cart items?","Yes", "No"))
        {
            Items.Clear();
            RecalculateTotalAmount();
            await toastService.DisplayToast("Cart emptied!");
        }
    }

    private void RecalculateTotalAmount()
    {
        Thread.Sleep(10000);
        TotalAmount = Items.Sum(x => x.Amount);
    }

}