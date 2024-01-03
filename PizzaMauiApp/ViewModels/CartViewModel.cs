using PizzaMauiApp.Models;
using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;
using PizzaMauiApp.ViewModels.Models;

namespace PizzaMauiApp.ViewModels;

public partial class CartViewModel(
    INavigationService navigationService,
    IDialogService dialogService, 
    IToastService toastService,
    IPizzaService pizzaService,
    ICartService cartService) : ViewModelBase
{
    public ObservableCollection<CartPizzaModel> Items { get; set; } = new();

    [ObservableProperty] private double _totalAmount;
    [ObservableProperty] private bool _hasItemsInCart = true;

    public override async Task ExecuteOnViewModelInit()
    {
        Items.Clear();
        
        var cartItems = cartService.GetCart();
        foreach (var itemInCart in cartItems)
        {
            var pizzaItem = await pizzaService.GetById(itemInCart.Key);
            if(pizzaItem == null)
                continue;
            
            Items.Add(new CartPizzaModel
                {
                    Id = itemInCart.Key,
                    Name = pizzaItem.Name,
                    Image = pizzaItem.Image,
                    Price = pizzaItem.Price,
                    Quantity = itemInCart.Value,
                    Amount = pizzaItem.Price * itemInCart.Value
                }
            );
        }
        HasItemsInCart = Items.Any();
        
        RecalculateTotalAmount();
    }

    [RelayCommand]
    private void OnRemoveFromCart(Guid pizzaItemId)
    {
        var items = Items.Where(i => i.Id == pizzaItemId).ToList();
        foreach (var item in items)
        {
            Items.Remove(item);
            cartService.RemoveAllFromCart(item.Id);
        }

        if(!Items.Any())
            HasItemsInCart = false;
        
        RecalculateTotalAmount();
    }

    [RelayCommand]
    private async Task OnClearCart()
    {
        if (await dialogService.DisplayConfirm("Confirm clear cart?", "Do you really want to clear the cart items?","Yes", "No"))
        {
            Items.Clear();
            cartService.ClearCart();
            
            RecalculateTotalAmount();
            await toastService.DisplayToast("Cart emptied!");

            HasItemsInCart = false;
        }
    }
    
    [RelayCommand]
    private async Task OnNavigateHome()
    {
        await navigationService.NavigateToPage<HomePage>();
    }

    private void RecalculateTotalAmount()
    {
        TotalAmount = Items.Sum(x => x.Amount);
    }

}