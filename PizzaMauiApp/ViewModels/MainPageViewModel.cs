using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class MainPageViewModel(INavigationService navigationService) : ViewModelBase
{
    [RelayCommand]
    private async Task OnGetStarted()
    {
        await navigationService.NavigateToPage<HomePage>();
    }
}