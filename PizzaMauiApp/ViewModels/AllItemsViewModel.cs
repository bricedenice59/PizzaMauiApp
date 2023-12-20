using PizzaMauiApp.Models;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class AllItemsViewModel(IPizzaService pizzaService) : ViewModelBase
{
    public ObservableCollection<Pizza> AllItems { get; set; } 

    [ObservableProperty] 
    private bool _searchBarVisible;
    
    [ObservableProperty] 
    private bool _isSearching;
    
    [ObservableProperty] 
    private bool _isLoading;
    
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SearchItemsCommand))] 
    private string? _searchKeyword;

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
        AllItems = allItems.ToObservableCollection();
    }
    
    [RelayCommand]
    private async Task SearchItems()
    {
        IsSearching = true;
        
        AllItems.Clear();
        var items = await pizzaService.Lookup(SearchKeyword);
        foreach (var item in items)
        {
            AllItems.Add(item);
        }
        
        IsSearching = false;
    }
}