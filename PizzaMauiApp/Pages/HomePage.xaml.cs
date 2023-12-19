using PizzaMauiApp.Services;
using PizzaMauiApp.ViewModels;

namespace PizzaMauiApp.Pages;

public partial class HomePage : ContentPage
{
    public HomePage(IDIService diService)
    {
        BindingContext = diService.ResolveViewModel<HomePageViewModel>();
        InitializeComponent();
    }
}