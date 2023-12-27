using System.Collections.Specialized;
using PizzaMauiApp.Services;
using PizzaMauiApp.ViewModels;

namespace PizzaMauiApp.Pages;

public partial class AllItemsPage : ContentPage
{
    private readonly AllItemsViewModel _vm;
    public AllItemsPage(IDIService diService)
    {
        _vm = diService.ResolveViewModel<AllItemsViewModel>();
        BindingContext = _vm;
        _vm.AllItems.CollectionChanged += OnCollectionChanged; 
        InitializeComponent();
    }

    void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        //only set searchbar focus when there is at least one element to be displayed
        if (e.Action != NotifyCollectionChangedAction.Add) return;

        if (_vm.FromSearch)
        {
            searchBar.Focus();
        }
        _vm.AllItems.CollectionChanged -= OnCollectionChanged;
    }
}