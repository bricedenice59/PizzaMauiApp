using PizzaMauiApp.Services;
using PizzaMauiApp.ViewModels;

namespace PizzaMauiApp.Pages;

public partial class AllItemsPage : ContentPage
{
    private readonly AllItemsViewModel _vm;
    public AllItemsPage(IDIService diService)
    {
        _vm = diService.ResolveViewModel<AllItemsViewModel>()!;
        BindingContext = _vm;
        Appearing += OnAppearing;
        InitializeComponent();
    }

    async void OnAppearing(object? sender, EventArgs e)
    {
        if (_vm.FromSearch)
        {
            await Task.Delay(100);
            searchBar.Focus();
        }
    }
}