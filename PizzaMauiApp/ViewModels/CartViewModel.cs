using CommunityToolkit.Mvvm.Messaging;
using PizzaMauiApp.Messages;
using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;
using PizzaMauiApp.ViewModels.Models;

namespace PizzaMauiApp.ViewModels;

public partial class CartViewModel : ViewModelBase
{
    #region Fields
    private readonly IPizzaService _pizzaService;
    private readonly IToastService _toastService;
    private readonly ICartService _cartService;
    private readonly IDialogService _dialogService;
    #endregion
    
    #region Ctor

    public CartViewModel(
        IPizzaService pizzaService,
        IDialogService dialogService, 
        IToastService toastService,
        ICartService cartService)
    {
        _pizzaService = pizzaService;
        _dialogService = dialogService;
        _toastService = toastService;
        _cartService = cartService;
    }
    #endregion
    
    #region Properties
    public ObservableCollection<CartPizzaModel> Items { get; set; } = new();

    [ObservableProperty] private double _totalAmount;
    [ObservableProperty] private bool _hasItemsInCart;
    #endregion
    
    #region Commands
    
    [RelayCommand]
    private async Task OnFetchData()
    {
        Items.Clear();
        
        var cartItems = await _cartService
            .GetAllFromCart(CancellationToken.None)
            .ConfigureAwait(false);
        
        foreach (var itemInCart in cartItems)
        {
            var pizzaItem = await _pizzaService
                .GetById(itemInCart.Key)
                .ConfigureAwait(false);
             
            if(pizzaItem == null)
                continue;
            
            Items.Add(new CartPizzaModel
                {
                    Id = itemInCart.Key,
                    Name = pizzaItem.Name,
                    Image = pizzaItem.MainImageUrl,
                    Price = pizzaItem.PriceWithExcludedVAT,
                    Quantity = itemInCart.Value,
                    Amount = pizzaItem.PriceWithExcludedVAT * itemInCart.Value
                }
            );
        }
        HasItemsInCart = Items.Any();
        
        RecalculateTotalAmount();
    }
    
    [RelayCommand]
    private async Task OnRemoveFromCart(Guid pizzaItemId)
    {
        var items = Items.Where(i => i.Id == pizzaItemId).ToList();
        foreach (var item in items)
        {
            if(await _cartService
                   .RemoveAllFromCart(item.Id, CancellationToken.None)
                   .ConfigureAwait(false))
                Items.Remove(item);
        }

        if(!Items.Any())
            HasItemsInCart = false;
        
        RecalculateTotalAmount();
    }

    [RelayCommand]
    private async Task OnClearCart()
    {
        if (await _dialogService.DisplayConfirm("Confirm clear cart?", "Do you really want to clear the cart items?","Yes", "No"))
        {
            if (await _cartService
                    .ClearCart(CancellationToken.None)
                    .ConfigureAwait(false))
            {
                Items.Clear();

                RecalculateTotalAmount();
                await _toastService.DisplayToast("Cart emptied!");

                HasItemsInCart = false;
            }
            else await _toastService.DisplayToast("Technical error, cart could not be emptied.");
        }
    }
    
    [RelayCommand]
    private void OnNavigateHome()
    {
        WeakReferenceMessenger.Default.Send(new ShellRouteMessage(new ShellRoute
        {
            RouteName = nameof(HomePage)
        }));
    }

    #endregion

    #region Methods
    private void RecalculateTotalAmount()
    {
        TotalAmount = Items.Sum(x => x.Amount);
    }
    #endregion
}