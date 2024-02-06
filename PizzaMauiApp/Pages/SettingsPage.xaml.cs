using PizzaMauiApp.Services;
using PizzaMauiApp.ViewModels;

namespace PizzaMauiApp.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(IDIService diService)
    {
        BindingContext = diService.ResolveViewModel<SettingsPageViewModel>();
        InitializeComponent();
    }
}