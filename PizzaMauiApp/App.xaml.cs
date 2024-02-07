using CommunityToolkit.Mvvm.Messaging;
using PizzaMauiApp.Messages;
using PizzaMauiApp.Services;
using PizzaMauiApp.ViewModels.Models;

namespace PizzaMauiApp;

public partial class App : Application
{
    public App(INavigationService navigationService)
    {
        InitializeComponent();
        
        MainPage = new AppShell(navigationService);
        WeakReferenceMessenger.Default.Send(new HasAuthenticatedMessage(true));
    }

    private static bool IsAuthenticated => Preferences.Get(PreferencesStorageModel.UserHasAuthenticated, false);
}