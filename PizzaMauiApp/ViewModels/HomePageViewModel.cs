using PizzaMauiApp.Models;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class HomePageViewModel(IPizzaService pizzaService) : ViewModelBase
{
    private readonly IPizzaService _pizzaService = pizzaService;
    
    public ObservableCollection<Pizza> PopularPizzasCollection { get; set; } = pizzaService.GetPopular().ToObservableCollection();
    
    #region Commands
    
    [RelayCommand]
    public async Task OnGetBestOffer()
    {

    }
    
    [RelayCommand]
    public async Task OnLookup()
    {

    }
    
    [RelayCommand]
    public async Task OnViewAll()
    {

    }
    
    #endregion
}