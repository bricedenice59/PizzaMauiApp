using PizzaMauiApp.ViewModels;

namespace PizzaMauiApp.Services;

public interface IDIService
{
    T? ResolveViewModel<T>() where T : ViewModelBase;
    T? ResolveView<T>() where T : Page;
    T? Resolve<T>();
}

public class DIService : IDIService
{
    private readonly IServiceProvider _services;
    
    public DIService(IServiceProvider services)
        => _services = services;

    public T? ResolveViewModel<T>() where T : ViewModelBase
        => _services.GetService<T>();
    
    public T? ResolveView<T>() where T : Page
        => _services.GetService<T>();
    
    public T? Resolve<T>()
        => _services.GetService<T>();
}