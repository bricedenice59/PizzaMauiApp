using PizzaMauiApp.Services;
using PizzaMauiApp.ViewModels;

namespace PizzaMauiApp.Pages;

public partial class MainPage : ContentPage
{
    private readonly INavigationService _navigationService;
    public MainPage(IDIService diService, INavigationService navigationService)
    {
        BindingContext = diService.ResolveViewModel<MainPageViewModel>();
        _navigationService = navigationService;
        InitializeComponent();
        Appearing+= OnAppearing;
    }
    
    private async void OnAppearing(object? sender, EventArgs e)
    {
        var vm = BindingContext as MainPageViewModel;
        if (vm!.IsAuthenticated())
            await _navigationService.NavigateToPage<HomePage>();
    }
}