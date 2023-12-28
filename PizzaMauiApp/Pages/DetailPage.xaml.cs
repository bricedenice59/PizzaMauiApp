using PizzaMauiApp.Services;
using PizzaMauiApp.ViewModels;

namespace PizzaMauiApp.Pages;

public partial class DetailPage : ContentPage
{
    public DetailPage(IDIService diService)
    {
        BindingContext = diService.ResolveViewModel<DetailPageViewModel>();
        InitializeComponent();
    }
}