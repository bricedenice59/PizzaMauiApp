using PizzaMauiApp.Models;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class AllItemsViewModel(IPizzaService pizzaService) : ViewModelBase
{
    public ObservableCollection<Pizza> AllItems { get; set; } = new();

    [ObservableProperty] 
    private bool _searchBarVisible;
    
    [ObservableProperty] 
    private bool _isSearching;
    
    [ObservableProperty] 
    private bool _isLoading;

    protected override async Task ExecuteOnLoad()
    {
        IsLoading = true;
        
        await Task.Delay(2000); //just simulate a long operation for time being
        await LoadAllItems();
        
        IsLoading = false;
    }

    public override Task OnNavigatingTo(object? parameter)
    {
        if (parameter is bool)
            SearchBarVisible = bool.Parse(parameter.ToString()!);
        return base.OnNavigatingTo(parameter);
    }
    
    [RelayCommand]
    private async Task LoadAllItems()
    {
        var allItems = await pizzaService.GetAll();
        foreach (var item in allItems)
        {
            AllItems.Add(item);
        }
    }
    
    [RelayCommand]
    private async Task SearchItems(string keyword)
    {
        IsSearching = true;
        
        AllItems.Clear();
        var items = await pizzaService.Lookup(keyword);
        foreach (var item in items)
        {
            AllItems.Add(item);
        }
        
        IsSearching = false;
    }
}