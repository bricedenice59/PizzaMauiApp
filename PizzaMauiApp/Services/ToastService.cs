using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace PizzaMauiApp.Services;

public interface IToastService
{
    Task DisplayToast(string message, ToastDuration toastDuration = ToastDuration.Short);
}
public class ToastService : IToastService
{
    public async Task DisplayToast(string message, ToastDuration toastDuration = ToastDuration.Short)
    {
        await Toast.Make(message, toastDuration).Show();
    }
}