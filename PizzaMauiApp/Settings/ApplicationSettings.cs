namespace PizzaMauiApp.Settings;

public class ApplicationSettings
{
    public WebAPISettings WebAPI { get; set; } = null!;

    public class WebAPISettings
    {
        public string EndpointUrl { get; set; } = null!;
        public string GetAllEndpointName { get; set; } = null!;
    }
}
