using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;
using PizzaMauiApp.ViewModels;
using Serilog;
using Serilog.Events;

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
        
        builder.Services.SetupSerilog();
        builder.Services.AddTransient<MainPage, MainPageViewModel>();
        builder.Services.AddTransient<HomePage, HomePageViewModel>();
        builder.Services.AddTransient<AllItemsPage, AllItemsViewModel>();
        builder.Services.AddTransient<DetailPage, DetailPageViewModel>();
        builder.Services.AddTransient<CartViewPage, CartViewModel>();
        
        builder.Services.AddSingleton<IDIService, DIService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddSingleton<IPizzaService, PizzaService>();
        builder.Services.AddSingleton<ICartService, CartService>();
        builder.Services.AddSingleton<IDialogService, DialogService>();
        builder.Services.AddSingleton<IToastService, ToastService>();
        builder.Services.AddSingleton<IRequestApiService, RequestApiService>();
        
        builder.Services.AddLogging(
            configure =>
            {
                configure.AddSerilog(dispose: true);
            }
        );
        
        builder.Services.AddSingleton<IAppSettings, AppSettings>();

        return builder.Build();
    }
    
    private static void SetupSerilog(this IServiceCollection serviceCollection)
    {
        var flushInterval = new TimeSpan(0, 0, 1);
        var file = Path.Combine(FileSystem.AppDataDirectory, "PizzaMauiApp.log");

        var logger = Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.File(file, 
                flushToDiskInterval: flushInterval,
                encoding: System.Text.Encoding.UTF8,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 10)
            .CreateLogger();
        
        serviceCollection.AddSingleton<ILogger>(logger);
    }
}