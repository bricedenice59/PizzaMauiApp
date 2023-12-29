using PizzaMauiApp.Models;
using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class AllItemsViewModel(IPizzaService pizzaService, INavigationService navigationService) : ViewModelBase
{
    public ObservableCollection<Pizza> AllItems { get; set; } = 
        pizzaService.GetAll().GetAwaiter().GetResult().ToObservableCollection();

    [ObservableProperty] 
    private bool _fromSearch;
    
    [ObservableProperty] 
    private bool _isLoading;
    
    public override Task OnNavigatingTo(object? parameter)
    {
        if (parameter is bool)
            FromSearch = bool.Parse(parameter.ToString());
        return base.OnNavigatingTo(parameter);
    }
    
    [RelayCommand]
    private async Task SearchItems(string keyword)
    {
        AllItems.Clear();

        IsLoading = true;
        
        var items = await pizzaService.Lookup(keyword);
        foreach (var item in items)
        {
            AllItems.Add(item);
        }

        IsLoading = false;
    }
    
    [RelayCommand]
    private async Task OnViewMore(Pizza pizzaItem)
    {
        await navigationService.NavigateToPage<DetailPage>(pizzaItem);
    }
}