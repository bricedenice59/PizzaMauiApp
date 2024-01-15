using System.Reflection;
using Microsoft.Extensions.Configuration;
using PizzaMauiApp.Settings;

namespace PizzaMauiApp.Services;

public interface IAppSettings
{
    ApplicationSettings Settings { get; set; }
}

public class AppSettings : IAppSettings
{
    private const string AppSettingsJsonPath = "PizzaMauiApp.appsettings.json";
    public ApplicationSettings Settings { get; set; }

    public AppSettings(ILogger logger)
    {
        Settings = new ApplicationSettings();
        logger.Information("Getting application settings... from appsettings.json");

        var ass = Assembly.GetExecutingAssembly();
        var settings = ass.GetManifestResourceStream(AppSettingsJsonPath);

        if (settings == null)
        {
            logger.Error($"Could not read embedded file {AppSettingsJsonPath}");
            throw new FileNotFoundException(AppSettingsJsonPath);
        }

        // Add appsettings.json file
        var configRoot = new ConfigurationBuilder()
            .AddJsonStream(settings)
            .Build();

        // Bind to Config class
        configRoot.Bind(this);

        var appSettings = configRoot.GetRequiredSection("ApplicationSettings").Get<ApplicationSettings>();
        if (appSettings == null)
            throw new($"Could not read 'Settings' key from {AppSettingsJsonPath}");

        Settings = appSettings;
    }

}