using PizzaMauiApp.Models;
using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class DetailPageViewModel(INavigationService navigationService, 
    IToastService toastService,
    ICartService cartService) : ViewModelBase
{
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

    public void SetQuantityOnPageChanged()
    {
        if (PizzaItem != null &&
            cartService.GetCart().TryGetValue(PizzaItem.Id, out int quantity))
            Quantity = quantity;
        else Quantity = 0;
    }

    [RelayCommand]
    private async Task OnGoBack()
    {
        await navigationService.NavigateBack();
    }

    [RelayCommand]
    private void OnIncrementQuantity()
    {
        if (PizzaItem == null)
            return;

        Quantity++;
        
        cartService.AddToCart(PizzaItem.Id);
    }

    [RelayCommand]
    private void OnDecrementQuantity()
    {
        if (PizzaItem == null)
            return;
        if (Quantity >= 1)
            Quantity--;
        
        cartService.RemoveOneFromCart(PizzaItem.Id);
    }
    
    [RelayCommand]
    private async Task OnViewCart()
    {
        if (PizzaItem == null)
            return;

        if(Quantity == 0)
        {
            await toastService.DisplayToast("Please select a quantity more than 0");
            return;
        }

        await navigationService.NavigateToPage<CartViewPage>();
    }
}