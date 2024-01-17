using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class MainPageViewModel : ViewModelBase
{
    #region Fields
    private readonly INavigationService _navigationService;
    #endregion
    
    #region Properties
    
    [ObservableProperty]
    private bool _isLoginVisible;
    [ObservableProperty]
    private string? _username;
    [ObservableProperty]
    private string? _password;
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
    [RelayCommand]
    private void OnGoRegisterOrLogin()
    {
        IsLoginVisible = false;
        Username = string.Empty;
        Password = string.Empty;
    }
    [RelayCommand]
    private void OnShowLogin()
    {
        IsLoginVisible = true;
        Username = string.Empty;
        Password = string.Empty;
    }
    [RelayCommand]
    private void OnShowSignup()
    {
        IsLoginVisible = false;
    }
    #endregion

    #region Methods
    
    public async Task<bool> IsAuthenticated()
    {
        var hasAuthentication = await SecureStorage.GetAsync("hasAuthentication");
        return !string.IsNullOrEmpty(hasAuthentication);
    }
    
    #endregion
}