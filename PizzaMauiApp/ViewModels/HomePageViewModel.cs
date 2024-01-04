using PizzaMauiApp.Models;
using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class HomePageViewModel : ViewModelBase
{
    #region Fields
    private readonly INavigationService _navigationService;
    private readonly IPizzaService _pizzaService;
    #endregion
    
    #region Ctor

    public HomePageViewModel(
        INavigationService navigationService,
        IPizzaService pizzaService)
    {
        _navigationService = navigationService;
        _pizzaService = pizzaService;
    }
    #endregion
    
    #region Properties
    [ObservableProperty] 
    private bool _isLoading;

    public ObservableCollection<Pizza> PopularPizzas { get; set; } = [];
    
    #endregion

    #region Commands
    
    [RelayCommand]
    public Task OnGetBestOffer()
    {
        throw new NotImplementedException();
    }
    
    [RelayCommand]
    private async Task OnLookup()
    {
        await _navigationService.NavigateToPage<AllItemsPage>(true);
    }
    
    [RelayCommand]
    private async Task OnViewAll()
    {
        await _navigationService.NavigateToPage<AllItemsPage>();
    }
    
    [RelayCommand]
    private async Task OnViewMore(Pizza pizza)
    {
        await _navigationService.NavigateToPage<DetailPage>(pizza);
    }
    
    #endregion

    #region Overrides

    public override async Task ExecuteOnViewModelInit()
    {
        IsLoading = true;
        await _pizzaService.GetAll();
        var populars = await _pizzaService.GetPopular();
        foreach (var popularPizza in populars)
        {
            PopularPizzas.Add(popularPizza);
        }
        
        IsLoading = false;
    }

    #endregion
}