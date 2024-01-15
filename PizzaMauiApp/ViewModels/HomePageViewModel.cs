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
    private async Task OnFetchDataWhenNoResult()
    {
        LoadingError = false;
        
        await FetchDataAndPopulate();
        
        LoadingError = !PopularPizzas.Any();
    }
    
    #endregion

    #region Overrides

    public override async Task ExecuteOnViewModelInit()
    {
        IsLoading = true;

        await FetchDataAndPopulate();
        
        IsLoading = false;

        LoadingError = !PopularPizzas.Any();
    }

    #endregion

    private async Task FetchDataAndPopulate()
    {
        var populars = await _pizzaService.GetPopular();
        foreach (var popularPizza in populars)
        {
            PopularPizzas.Add(popularPizza);
        }
    }
}