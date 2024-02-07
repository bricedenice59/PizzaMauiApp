using CommunityToolkit.Mvvm.Messaging.Messages;

namespace PizzaMauiApp.Messages;


public class HasAuthenticatedMessage : ValueChangedMessage<bool>
{
    public HasAuthenticatedMessage(bool value) : base(value)
    {

    }
}