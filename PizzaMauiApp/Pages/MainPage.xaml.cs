using PizzaMauiApp.Services;
using PizzaMauiApp.ViewModels;

namespace PizzaMauiApp.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(IDIService diService)
    {
        BindingContext = diService.ResolveViewModel<MainPageViewModel>();
        InitializeComponent();
        Appearing+= OnAppearing;
    }
    
    private async void OnAppearing(object? sender, EventArgs e)
    {
        var vm = BindingContext as MainPageViewModel;
        if (await vm!.IsAuthenticated())
            await vm.LoginOrSignupCommand.ExecuteAsync(null);
    }
}