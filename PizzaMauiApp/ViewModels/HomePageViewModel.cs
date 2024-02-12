using CommunityToolkit.Mvvm.Messaging;
using PizzaMauiApp.Messages;
using PizzaMauiApp.Models;
using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public partial class HomePageViewModel : ViewModelBase
{
    #region Fields
    private readonly ILogger _logger;
    private readonly INavigationService _navigationService;
    private readonly IPizzaService _pizzaService;

    #endregion
    
    #region Ctor

    public HomePageViewModel(
        ILogger logger,
        INavigationService navigationService,
        IPizzaService pizzaService)
    {
        _logger = logger;
        _navigationService = navigationService;
        _pizzaService = pizzaService;

    }
    #endregion
    
    #region Properties
    [ObservableProperty] 
    private bool _isLoading;
    
    [ObservableProperty] 
    [NotifyCanExecuteChangedFor(nameof(GetBestOfferCommand))]
    [NotifyCanExecuteChangedFor(nameof(LookupCommand))]
    [NotifyCanExecuteChangedFor(nameof(ViewAllCommand))]
    [NotifyCanExecuteChangedFor(nameof(ViewMoreCommand))]
    private bool _loadingError;

    private bool CanInteract => !LoadingError;

    public ObservableCollection<Pizza> PopularPizzas { get; set; } = [];
    
    #endregion

    #region Commands
    
    [RelayCommand(CanExecute = nameof(CanInteract))]
    public Task OnGetBestOffer()
    {
        throw new NotImplementedException();
    }
    
    [RelayCommand(CanExecute = nameof(CanInteract))]
    private async Task OnLookup()
    {
        await _navigationService.NavigateToPage<AllItemsPage>(true);
    }
    
    [RelayCommand(CanExecute = nameof(CanInteract))]
    private async Task OnViewAll()
    {
        await _navigationService.NavigateToPage<AllItemsPage>();
    }
    
    [RelayCommand(CanExecute = nameof(CanInteract))]
    private async Task OnViewMore(Pizza pizza)
    {
        await _navigationService.NavigateToPage<DetailPage>(pizza);
    }
    
    [RelayCommand]
    private async Task OnFetchData()
    {
        PopularPizzas.Clear();
        LoadingError = false;
        IsLoading = true;
        
        await FetchDataAndPopulate().ConfigureAwait(true);
        
        IsLoading = false;
        
        LoadingError = !PopularPizzas.Any();
    }
    
    #endregion

    private async Task FetchDataAndPopulate()
    {
        IEnumerable<Pizza>? populars = await _pizzaService.GetPopular();
        if (populars == null) return;
        
        foreach (var popularPizza in populars)
        {
            Console.WriteLine(popularPizza.Name);
            PopularPizzas.Add(popularPizza);
        }
        Console.WriteLine();
    }
}