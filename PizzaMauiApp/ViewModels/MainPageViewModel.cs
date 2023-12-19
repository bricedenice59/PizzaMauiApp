using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class MainPageViewModel(INavigationService navigationService) : ViewModelBase
{
    [RelayCommand]
    public async Task OnGetStarted()
    {
        await navigationService.NavigateToPage<HomePage>();
    }
}