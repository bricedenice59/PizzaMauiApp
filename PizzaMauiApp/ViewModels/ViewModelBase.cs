using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public class ViewModelBase
{
    public virtual Task OnNavigatingTo(object? parameter)
        => Task.CompletedTask;

}