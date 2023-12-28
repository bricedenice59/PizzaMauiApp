using PizzaMauiApp.ViewModels;

namespace PizzaMauiApp.Services;

public interface INavigationService
{
    Task NavigateToPage<T>(object? parameter = null, bool isAnimated = true) where T : Page;
    Task NavigateBack(object? parameter = null);
}

public class NavigationService(IDIService services) : INavigationService
{
    private INavigation Navigation
    {
        get
        {
            var navigation = Application.Current?.MainPage?.Navigation;
            if (navigation is not null)
                return navigation;
            else
            {
                throw new Exception();
            }
        }
    }

    public async Task NavigateToPage<T>(object? parameter = null, bool isAnimated = true) where T : Page
    {
        var toPage = services.ResolveView<T>();

        if (toPage is not null && toPage.BindingContext is not null)
        {
            var vmBase = toPage.BindingContext as ViewModelBase;
            if (vmBase is null)
                throw new InvalidOperationException($"BindingContext for page {toPage} is not set!"); 
            
            await vmBase.OnNavigatingTo(parameter);
            
            await Navigation.PushAsync(toPage, isAnimated);
        }
        else
            throw new InvalidOperationException($"Unable to resolve type {typeof(T).FullName}");
    }

    public async Task NavigateBack(object? parameter = null)
    {
        var numberOfPages = Navigation.NavigationStack.Count(x => x != null);
        if(numberOfPages == 0)
            throw new InvalidOperationException($"Unable to navigate back...");
        
        var previousPage = Navigation.NavigationStack[numberOfPages -1];
        var vmBase = previousPage.BindingContext as ViewModelBase;

        await vmBase!.OnNavigatingFrom(parameter);
        await Navigation.PopAsync( true);
    }
}