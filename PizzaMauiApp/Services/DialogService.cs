namespace PizzaMauiApp.Services;

public interface IDialogService
{
    Task<bool> DisplayConfirm(string title, string message, string accept, string cancel);
}
public class DialogService : IDialogService
{
    public async Task<bool> DisplayConfirm(string title, string message, string accept, string cancel)
    {
        return await Shell.Current.DisplayAlert(title, message, accept, cancel);
    }
}