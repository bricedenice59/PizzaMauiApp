namespace PizzaMauiApp.Settings;

public class ApplicationSettings
{
    public WebAPISettings WebAPI { get; set; } = null!;

    public class WebAPISettings
    {
        public string EndpointUrl { get; set; } = null!;
        public string GetAllEndpointName { get; set; } = null!;
        
        public string LoginEndpointName { get; set; } = null!;
        
        public string RegisterEndpointName { get; set; } = null!;
        
        public string RefreshTokenEndpointName { get; set; } = null!;
        
        public string CartEndpoint { get; set; } = null!;
    }
}
