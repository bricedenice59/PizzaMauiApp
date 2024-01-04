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
                return Quantity * PizzaItem.Price;
            
            return 0;
        }
    }
    #endregion

    #region Overrides
    public override Task OnNavigatingFrom(object? parameter)
    {
        SetQuantityOnPageChanged();
        return base.OnNavigatingFrom(parameter);
    }

    public override Task OnNavigatingTo(object? parameter)
    {
        if (parameter is not null)
            PizzaItem = parameter as Pizza;
        return base.OnNavigatingTo(parameter);
    }

    public override Task ExecuteOnViewModelInit()
    {
        SetQuantityOnPageChanged();
        return base.ExecuteOnViewModelInit();
    }
    #endregion
    
    #region Commands
    [RelayCommand]
    private async Task OnGoBack()
    {
        await _navigationService.NavigateBack();
    }

    [RelayCommand]
    private void OnIncrementQuantity()
    {
        if (PizzaItem == null)
            return;

        Quantity++;
        
        _cartService.AddToCart(PizzaItem.Id);
    }

    [RelayCommand]
    private void OnDecrementQuantity()
    {
        if (PizzaItem == null)
            return;
        if (Quantity >= 1)
            Quantity--;
        
        _cartService.RemoveOneFromCart(PizzaItem.Id);
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
    private void SetQuantityOnPageChanged()
    {
        if (PizzaItem != null &&
            _cartService.GetCart().TryGetValue(PizzaItem.Id, out int quantity))
            Quantity = quantity;
        else Quantity = 0;
    }
    #endregion
}