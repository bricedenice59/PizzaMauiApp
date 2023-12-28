using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class MainPageViewModel(IPizzaService pizzaService, INavigationService navigationService) : ViewModelBase
{
    [RelayCommand]
    public async Task OnGetStarted()
    {
        //run here long operation calls and cache results
        await pizzaService.GetAll();
        var t = await pizzaService.GetPopular();
        await navigationService.NavigateToPage<HomePage>();
    }
}