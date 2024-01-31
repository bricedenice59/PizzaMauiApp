using PizzaMauiApp.Models;
using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class DetailPageViewModel : ViewModelBase
{
    #region Fields
    private readonly INavigationService _navigationService;
    private readonly IToastService _toastService;
    private readonly ICartService _cartService;
    #endregion
    
    #region Ctor

    public DetailPageViewModel(
        INavigationService navigationService, 
        IToastService toastService,
        ICartService cartService)
    {
        _navigationService = navigationService;
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
        await SetQuantityOnPageChanged();
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
        await SetQuantityOnPageChanged();
        await base.ExecuteOnViewModelInit();
    }
    #endregion
    
    #region Commands
    [RelayCommand]
    private async Task OnGoBack()
    {
        await _navigationService.NavigateBack();
    }

    [RelayCommand]
    private async Task OnIncrementQuantity()
    {
        if (PizzaItem == null)
            return;
        
        if(await _cartService.AddOneToCart(PizzaItem.Id))
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
            if(await _cartService.RemoveOneFromCart(PizzaItem.Id))
                Quantity--;
            else await _toastService.DisplayToast("Technical error, could not remove pizza from the cart.");
        }
    }
    
    [RelayCommand]
    private async Task OnViewCart()
    {
        if (PizzaItem == null)
            return;

        if(Quantity == 0)
        {
            await _toastService.DisplayToast("Please select a quantity more than 0");
            return;
        }

        await _navigationService.NavigateToPage<CartViewPage>();
    }
    #endregion
    
    #region Methods
    private async Task SetQuantityOnPageChanged()
    {
        var allItems = await _cartService.GetAllFromCart();
        if (PizzaItem != null &&
            allItems.TryGetValue(PizzaItem.Id, out int quantity))
            Quantity = quantity;
        else Quantity = 0;
    }
    #endregion
}