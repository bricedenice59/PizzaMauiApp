using CommunityToolkit.Mvvm.Messaging;
using PizzaMauiApp.Messages;
using PizzaMauiApp.Models;
using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class DetailPageViewModel : ViewModelBase
{
    #region Fields
    private readonly IToastService _toastService;
    private readonly ICartService _cartService;
    #endregion
    
    #region Ctor

    public DetailPageViewModel(
        IToastService toastService,
        ICartService cartService)
    {
        _toastService = toastService;
        _cartService = cartService;
    }
    #endregion
    
    #region Properties
    [ObservableProperty] 
    private Pizza? _pizzaItem;
    
    [ObservableProperty, NotifyPropertyChangedFor(nameof(Amount))]
    private int _quantity;
    
    public double Amount
    {
        get
        {
            if (PizzaItem != null) 
                return Quantity * PizzaItem.PriceWithExcludedVAT;
            
            return 0;
        }
    }
    #endregion
    
    #region Overrides
    public override async Task OnNavigatingFrom(object? parameter)
    {
        await OnFetchData().ConfigureAwait(false);
        await base.OnNavigatingFrom(parameter);
    }

    public override Task OnNavigatingTo(object? parameter)
    {
        if (parameter is not null)
            PizzaItem = parameter as Pizza;
        return base.OnNavigatingTo(parameter);
    }

    public override async Task ExecuteOnViewModelInit()
    {
        await OnFetchData().ConfigureAwait(false);
        await base.ExecuteOnViewModelInit();
    }
    #endregion
    
    #region Commands
    
    [RelayCommand]
    private void OnGoBack()
    {
        WeakReferenceMessenger.Default.Send(new ShellRouteMessage(new ShellRoute
        {
            RouteName = nameof(HomePage)
        }));
    }

    [RelayCommand]
    private async Task OnIncrementQuantity()
    {
        if (PizzaItem == null)
            return;
        
        if(await _cartService
               .AddOneToCart(PizzaItem.Id, CancellationToken.None)
               .ConfigureAwait(false))
            Quantity++;
        else await _toastService.DisplayToast("Technical error, could not add pizza to the cart.");
    }

    [RelayCommand]
    private async Task OnDecrementQuantity()
    {
        if (PizzaItem == null)
            return;
        if (Quantity >= 1)
        {
            if(await _cartService
                   .RemoveOneFromCart(PizzaItem.Id, CancellationToken.None)
                   .ConfigureAwait(false))
                Quantity--;
            else await _toastService.DisplayToast("Technical error, could not remove pizza from the cart.");
        }
    }
    
    [RelayCommand]
    private async Task OnFetchData()
    {
        var allItems = await _cartService.GetAllFromCart(CancellationToken.None);
        
        if (PizzaItem != null &&
            allItems.TryGetValue(PizzaItem.Id, out int quantity))
            Quantity = quantity;
        else Quantity = 0;
    }
    #endregion
}