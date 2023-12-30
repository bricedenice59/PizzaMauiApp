using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public class ViewModelBase : ObservableObject
{
    public virtual Task ExecuteOnViewModelInit()
        => Task.CompletedTask;
    public virtual Task OnNavigatingTo(object? parameter)
        => Task.CompletedTask;
    
    public virtual Task OnNavigatingFrom(object? parameter)
        => Task.CompletedTask;
}