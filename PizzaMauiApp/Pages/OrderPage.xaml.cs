using PizzaMauiApp.Services;
using PizzaMauiApp.ViewModels;

namespace PizzaMauiApp.Pages;

public partial class OrderPage : ContentPage
{
    public OrderPage(IDIService diService)
    {
        BindingContext = diService.ResolveViewModel<OrderPageViewModel>();
        InitializeComponent();
    }
}