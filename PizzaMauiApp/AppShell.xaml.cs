using CommunityToolkit.Mvvm.Messaging;
using PizzaMauiApp.Messages;
using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;


namespace PizzaMauiApp;

public partial class AppShell : Shell
{
    public AppShell(INavigationService navigationService)
    {
        InitializeComponent();
        
        WeakReferenceMessenger.Default.Register<HasAuthenticatedMessage>(this, async (_, m) =>
        {
            ShellContentMainPage.IsVisible = !m.Value;
            NavigationTabBar.IsVisible = m.Value;
            if(m.Value)
                await navigationService.NavigateToPage<HomePage>(); 
        });
        
        WeakReferenceMessenger.Default.Register<ShellRouteMessage>(this, async (_, m) =>
        {
            ShellNavigationQueryParameters? navigationParameter = null;
            if (m.Value.Parameter != null)
            {
                navigationParameter = new ShellNavigationQueryParameters
                {
                    { "param", m.Value.Parameter }
                };
            }

            //navigate backward
            if (m.Value.IsBackward)
            {
                if (navigationParameter != null)
                    await Shell.Current.GoToAsync($"./{m.Value.RouteName}", true, navigationParameter);
                else
                    await Shell.Current.GoToAsync($"./{m.Value.RouteName}", true);
            }
            else
            {
                if (navigationParameter != null)
                    await Shell.Current.GoToAsync($"//{m.Value.RouteName}", true, navigationParameter);
                else
                    await Shell.Current.GoToAsync($"//{m.Value.RouteName}", true);
            }
        });
    }
}