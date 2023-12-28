using PizzaMauiApp.Models;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class DetailPageViewModel(INavigationService navigationService) : ViewModelBase
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
}