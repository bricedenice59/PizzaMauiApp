using PizzaMauiApp.Models;
using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class HomePageViewModel(IPizzaService pizzaService, INavigationService navigationService) : ViewModelBase
{
    [ObservableProperty] 
    private bool _isLoading;
    public ObservableCollection<Pizza> PopularPizzas { get; set; } =
        pizzaService.GetPopular().GetAwaiter().GetResult().ToObservableCollection();

    #region Commands
    
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
    
    [RelayCommand]
    private async Task OnViewMore(Pizza pizza)
    {
        await navigationService.NavigateToPage<DetailPage>(pizza);
    }
    
    #endregion
}