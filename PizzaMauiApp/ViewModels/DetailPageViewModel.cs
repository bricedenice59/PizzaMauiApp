using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using PizzaMauiApp.Models;
using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class DetailPageViewModel(INavigationService navigationService, IToastService toastService) : ViewModelBase
{
    [ObservableProperty] private Pizza? _pizzaItem;

    public override Task OnNavigatingTo(object? parameter)
    {
        if (parameter is not null)
            PizzaItem = parameter as Pizza;
        return base.OnNavigatingTo(parameter);
    }

    [RelayCommand]
    private async Task OnGoBack()
    {
        await navigationService.NavigateBack();
    }

    [RelayCommand]
    private void OnIncrementQuantity()
    {
        if (PizzaItem != null) PizzaItem.Quantity++;
    }

    [RelayCommand]
    private void OnDecrementQuantity()
    {
        if (PizzaItem != null && PizzaItem.Quantity == 0)
            return;
        if (PizzaItem != null) 
            PizzaItem.Quantity--;
    }
    
    [RelayCommand]
    private async Task OnViewCart()
    {
        if (PizzaItem != null && PizzaItem.Quantity == 0)
        {
            await toastService.DisplayToast("Please select a quantity more than 0");
            return;
        }

        await navigationService.NavigateToPage<CartViewPage>();
    }
}