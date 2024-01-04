using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class MainPageViewModel : ViewModelBase
{
    #region Fields
    private readonly INavigationService _navigationService;
    #endregion
    
    #region Ctor

    public MainPageViewModel(
        INavigationService navigationService)
    {
        _navigationService = navigationService;
    }
    #endregion
    
    #region Commands
    [RelayCommand]
    private async Task OnGetStarted()
    {
        await _navigationService.NavigateToPage<HomePage>();
    }
    #endregion
}