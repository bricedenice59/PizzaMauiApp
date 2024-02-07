using CommunityToolkit.Mvvm.Messaging.Messages;

namespace PizzaMauiApp.Messages;

public class ShellRoute
{
    public required string RouteName { get; set; }
    public bool IsBackward { get; set; }
    
    public object? Parameter { get; set; }
}

public class ShellRouteMessage : ValueChangedMessage<ShellRoute>
{
    public ShellRouteMessage(ShellRoute route) : base(route)
    {

    }
}