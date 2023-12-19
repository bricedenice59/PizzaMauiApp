namespace PizzaMauiApp.ViewModels;

public class ViewModelBase : ObservableObject
{
    public virtual Task OnNavigatingTo(object? parameter)
        => Task.CompletedTask;

}