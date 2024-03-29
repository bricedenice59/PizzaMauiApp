using CommunityToolkit.Mvvm.Messaging;
using PizzaMauiApp.API.Dtos;
using PizzaMauiApp.Helpers.Crypto;
using PizzaMauiApp.Messages;
using PizzaMauiApp.Models;
using PizzaMauiApp.Services;
using PizzaMauiApp.ViewModels.Models;

namespace PizzaMauiApp.ViewModels;

public partial class MainPageViewModel : ViewModelBase
{
    #region Fields
    private readonly IAccountService _accountService;
    #endregion
    
    #region Properties

    [ObservableProperty] 
    private UserModel _userModel;
    
    //launch with "login view" when starting for the first time
    [ObservableProperty]
    private bool _isLoginVisible = true;
    
    [ObservableProperty]
    private bool _isProcessing;
    
    [ObservableProperty]
    private bool _loginOrSignupFailed;
    
    [ObservableProperty]
    private string? _loginOrSignupError;
    
    #endregion
    
    #region Ctor

    public MainPageViewModel(
        IAccountService accountService)
    {
        _accountService = accountService;
        UserModel = new UserModel();
        UserModel.Init(IsLoginVisible);
    }
    #endregion
    
    #region Commands
    [RelayCommand]
    private async Task OnLoginOrSignup()
    {
        var isEmailValid = UserModel.Email.Validate();
        var isPasswordValid = UserModel.Password.Validate();
        //in both case, we need both email address and password to be validated
        if (!isEmailValid && !isPasswordValid)
        {
            return;
        }

        LoginOrSignupFailed = false;
        IsProcessing = true;
        
        (bool, TokenModelDto?) loginOrSignupResult = new ValueTuple<bool, TokenModelDto?>(false, null);
        //login case
        if (IsLoginVisible)
        {
            loginOrSignupResult = await _accountService
                .LoginUserAsync(UserModel.Email.Value!, UserModel.Password.Value!, CancellationToken.None)
                .ConfigureAwait(true);
            
            if (!loginOrSignupResult.Item1)
            {
                LoginOrSignupFailed = true;
                LoginOrSignupError = "Login failed. Please check your credentials and try again.";
                return;
            }
        }
        //signup case
        else
        { 
            loginOrSignupResult = await _accountService
                .RegisterUserAsync(UserModel.Email.Value!, UserModel.Password.Value!, CancellationToken.None)
                .ConfigureAwait(true);
            
            if (!loginOrSignupResult.Item1)
            {
                LoginOrSignupFailed = true;
                LoginOrSignupError = "Signup failed. Please check your internet connection and try again.";
                return;
            }
        }
        //everything's ok
        Preferences.Set(PreferencesStorageModel.UserId, Sha256.GetHashString(UserModel.Email.Value!));
        Preferences.Set(PreferencesStorageModel.UserHasAuthenticated, true);
        Preferences.Set(PreferencesStorageModel.UserEmail, UserModel.Email.Value!);
        Preferences.Set(PreferencesStorageModel.UserToken, loginOrSignupResult.Item2?.AccessToken!);
        Preferences.Set(PreferencesStorageModel.UserRefreshToken, loginOrSignupResult.Item2?.RefreshToken!);
        IsProcessing = false;

        WeakReferenceMessenger.Default.Send(new HasAuthenticatedMessage(true));
    }

    [RelayCommand]
    private void OnShowLogin()
    {
        IsLoginVisible = true;
        UserModel.Init(IsLoginVisible);
    }
    
    [RelayCommand]
    private void OnShowSignup()
    {
        IsLoginVisible = false;
        UserModel.Init(IsLoginVisible);
    }
    
    [RelayCommand]
    private void OnValidatePassword()
    {
        UserModel.Password.Validate();
    }
    
    [RelayCommand]
    private void OnValidateEmail()
    {
        UserModel.Email.Validate();
    }
    
    [RelayCommand]
    private void OnCloseLoginSignupFailedWarning()
    {
       IsProcessing = false;
    }
    
    #endregion
}