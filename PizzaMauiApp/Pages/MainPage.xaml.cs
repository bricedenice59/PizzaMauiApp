using PizzaMauiApp.ViewModels;

namespace PizzaMauiApp.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(IDIService diService)
    {
        BindingContext = diService.ResolveViewModel<MainPageViewModel>();
        InitializeComponent();
    }
}