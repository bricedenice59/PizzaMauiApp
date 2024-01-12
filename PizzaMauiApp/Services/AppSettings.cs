using System.Reflection;
using Microsoft.Extensions.Configuration;
using PizzaMauiApp.Settings;
using ILogger = Serilog.ILogger;

namespace PizzaMauiApp.Services;

public interface IAppSettings
{
    ApplicationSettings? Settings { get; set; }
}

public class AppSettings : IAppSettings
{
    private const string AppSettingsJsonPath = "PizzaMauiApp.appsettings.json";
    public ApplicationSettings? Settings { get; set; } = new();

    public AppSettings(ILogger logger)
    {
        logger.Information("Getting application settings... from appsettings.json");
        
        var ass = Assembly.GetExecutingAssembly();
        var settings = ass.GetManifestResourceStream(AppSettingsJsonPath);
        
        try
        {
            if (settings == null)
            {
                logger.Error($"Could not read {AppSettingsJsonPath}");
                return;
            }
            
            // Add appsettings.json file
            var configRoot = new ConfigurationBuilder()
                .AddJsonStream(settings)
                .Build();

            // Bind to Config class
            configRoot.Bind(this);
                
            Settings = configRoot.GetRequiredSection("ApplicationSettings").Get<ApplicationSettings>();
            if (Settings == null)
                logger.Error("Could not get read 'Settings' key from appsettings.json file");
        }
        catch (Exception e)
        {
            logger.Error(e, e.Message);
        }
    }
    
}