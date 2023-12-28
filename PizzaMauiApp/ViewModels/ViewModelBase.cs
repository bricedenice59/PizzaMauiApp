using PizzaMauiApp.Services;

namespace PizzaMauiApp.ViewModels;

public class ViewModelBase : ObservableObject
{
    private RelayCommand _onLoadCommand;
    public virtual Task OnNavigatingTo(object? parameter)
        => Task.CompletedTask;
    
    public virtual Task OnNavigatingFrom(object? parameter)
        => Task.CompletedTask;

    public RelayCommand OnLoadCommand
    {
        get
        {
            return _onLoadCommand ?? (_onLoadCommand = new RelayCommand(async () => await ExecuteOnLoad()));
        }
    }

    /// <summary>
    /// Method executed on page load event.
    /// </summary>
    protected virtual async Task ExecuteOnLoad()
    {
    }

}