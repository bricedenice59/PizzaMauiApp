using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;
using PizzaMauiApp.ViewModels;

namespace PizzaMauiApp;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        }).UseMauiCommunityToolkit();
        
        builder.Services.AddTransient<MainPage, MainPageViewModel>();
        builder.Services.AddTransient<HomePage, HomePageViewModel>();
        builder.Services.AddTransient<AllItemsPage, AllItemsViewModel>();
        builder.Services.AddTransient<DetailPage, DetailPageViewModel>();
        
        builder.Services.AddSingleton<IDIService, DIService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IPizzaService, PizzaService>();
        
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}