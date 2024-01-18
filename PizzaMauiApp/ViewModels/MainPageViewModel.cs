using PizzaMauiApp.Helpers.ValidationRules;
using PizzaMauiApp.Models;
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
    private User _user;
    
    [ObservableProperty]
    private bool _isLoginVisible;
    
    #endregion
    
    #region Ctor

    public MainPageViewModel(
        INavigationService navigationService)
    {
        _navigationService = navigationService;
        User = new User();
        AddValidations();
    }
    #endregion
    
    #region Commands
    [RelayCommand]
    private async Task OnLoginOrSignup()
    {
        var isEmailValid = User.Email.Validate();
        var isPasswordValid = User.Password.Validate();
        if(isEmailValid && isPasswordValid)
            await _navigationService.NavigateToPage<HomePage>();
    }
    [RelayCommand]
    private void OnGoRegisterOrLogin()
    {
        IsLoginVisible = false;
        User.ResetValues();
    }
    [RelayCommand]
    private void OnShowLogin()
    {
        IsLoginVisible = true;
        User.ResetValues();
    }
    [RelayCommand]
    private void OnShowSignup()
    {
        IsLoginVisible = false;
        User.ResetValues();
    }
    [RelayCommand]
    private void OnValidatePassword()
    {
        User.Password.Validate();
    }
    [RelayCommand]
    private void OnValidateEmail()
    {
        User.Email.Validate();
    }
    #endregion

    #region Methods
    
    private void AddValidations()
    {
        User.Email.Validations.Add(new EmailRule<string> 
        { 
            ValidationMessage = "Email is not valid" 
        });

        User.Password.Validations.Add(new PasswordRule<string>
        { 
            ValidationMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non alphanumeric and at least 10 characters." 
        });
    }
    
    public async Task<bool> IsAuthenticated()
    {
        var hasAuthentication = await SecureStorage.GetAsync("hasAuthentication");
        return !string.IsNullOrEmpty(hasAuthentication);
    }
    
    #endregion
}