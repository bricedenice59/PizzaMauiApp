using PizzaMauiApp.Models;
using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class HomePageViewModel(IPizzaService pizzaService, INavigationService navigationService) : ViewModelBase
{
    [ObservableProperty] 
    private bool _isLoading;
    public ObservableCollection<Pizza> PopularPizzas { get; set; } = new();

    protected override async Task ExecuteOnLoad()
    {
        IsLoading = true;
        
        await Task.Delay(2000); //just simulate a long operation for time being
        await LoadAllPopularItems();
        
        IsLoading = false;
    }

    #region Commands
    
    private async Task LoadAllPopularItems()
    {
        var popularItems = await pizzaService.GetPopular();
        foreach (var popularItem in popularItems)
        {
            PopularPizzas.Add(popularItem);
        }
    }
    
    [RelayCommand]
    public async Task OnGetBestOffer()
    {

    }
    
    [RelayCommand]
    private async Task OnLookup()
    {
        await navigationService.NavigateToPage<AllItemsPage>(true);
    }
    
    [RelayCommand]
    private async Task OnViewAll()
    {
        await navigationService.NavigateToPage<AllItemsPage>();
    }
    
    #endregion
}