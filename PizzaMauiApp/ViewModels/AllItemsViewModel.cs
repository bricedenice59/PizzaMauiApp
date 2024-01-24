using PizzaMauiApp.Models;
using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class AllItemsViewModel : ViewModelBase
{
    #region Fields
    private readonly IPizzaService _pizzaService;
    private readonly INavigationService _navigationService;
    #endregion
    
    #region Ctor

    public AllItemsViewModel(
        IPizzaService pizzaService, 
        INavigationService navigationService)
    {
        _pizzaService = pizzaService;
        _navigationService = navigationService;
    }
    #endregion
    
    #region Properties
    public ObservableCollection<Pizza> AllItems { get; set; } = [];

    [ObservableProperty] 
    private bool _fromSearch;
    
    [ObservableProperty] 
    private bool _isLoading;
    
    [ObservableProperty] 
    private bool _hasNoResult;
    
    #endregion

    #region Overrides
    public override async Task ExecuteOnViewModelInit()
    {
        HasNoResult = false;
        IsLoading = true;
        
        var allPizzas = await _pizzaService.GetAll();
        if (allPizzas == null)
            return;
        foreach (var pizzaItem in allPizzas)
        {
            AllItems.Add(pizzaItem);
        }

        IsLoading = false;
    }

    public override Task OnNavigatingTo(object? parameter)
    {
        if (parameter is bool)
            FromSearch = bool.Parse(parameter.ToString()!);
        return base.OnNavigatingTo(parameter);
    }
    
    #endregion
    
    #region Commands
    [RelayCommand]
    private async Task SearchItems(string keyword)
    {
        HasNoResult = false;
        AllItems.Clear();

        IsLoading = true;
        
        var items = await _pizzaService.Lookup(keyword);
        if (items != null)
        {
            foreach (var item in items)
            {
                AllItems.Add(item);
            } 
        }

        IsLoading = false;
        HasNoResult = !AllItems.Any();
    }
    
    [RelayCommand]
    private async Task OnViewMore(Pizza pizzaItem)
    {
        await _navigationService.NavigateToPage<DetailPage>(pizzaItem);
    }
    #endregion
}