using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
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
        
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<HomePage>();
        
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<HomePageViewModel>();
        
        builder.Services.AddSingleton<IDIService, DIService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        
#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}