using PizzaMauiApp.Models;
using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class AllItemsViewModel(IPizzaService pizzaService, INavigationService navigationService) : ViewModelBase
{
    public ObservableCollection<Pizza> AllItems { get; set; } = [];

    [ObservableProperty] 
    private bool _fromSearch;
    
    [ObservableProperty] 
    private bool _isLoading;
    
    [ObservableProperty] 
    private bool _hasNoResult;

    public override async Task ExecuteOnViewModelInit()
    {
        HasNoResult = false;
        IsLoading = true;
        
        await pizzaService.GetAll();
        var allPizzas = await pizzaService.GetAll();
        foreach (var pizzaItem in allPizzas)
        {
            AllItems.Add(pizzaItem);
        }
        
        IsLoading = false;
    }

    public override Task OnNavigatingTo(object? parameter)
    {
        if (parameter is bool)
            FromSearch = bool.Parse(parameter.ToString());
        return base.OnNavigatingTo(parameter);
    }
    
    [RelayCommand]
    private async Task SearchItems(string keyword)
    {
        HasNoResult = false;
        AllItems.Clear();

        IsLoading = true;
        
        var items = await pizzaService.Lookup(keyword);
        foreach (var item in items)
        {
            AllItems.Add(item);
        }

        IsLoading = false;
        HasNoResult = !AllItems.Any();
    }
    
    [RelayCommand]
    private async Task OnViewMore(Pizza pizzaItem)
    {
        await navigationService.NavigateToPage<DetailPage>(pizzaItem);
    }
}