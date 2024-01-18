using System.Windows.Input;

namespace PizzaMauiApp.Behaviors;

//https://stackoverflow.com/questions/76299115/maui-net-update-binding-on-lost-focus-alike
public class EntryLostFocusBehavior : Behavior<Entry>
{
    #region TextProperty
    
    public static readonly BindableProperty TextProperty = BindableProperty.CreateAttached(
        "Text",
        typeof(string),
        typeof(EntryLostFocusBehavior),
        null,
        BindingMode.TwoWay);
    
    public static string GetText( Entry entry ) => (string)entry.GetValue( TextProperty );
    public static void SetText( Entry entry, string value ) => entry.SetValue( TextProperty, value );
    
    #endregion
    
    #region LostFocusCommand
    
    public static readonly BindableProperty LostFocusCommandProperty =
        BindableProperty.CreateAttached(
            "LostFocusCommand",
            typeof(ICommand),
            typeof(EntryLostFocusBehavior), 
            null,
            BindingMode.TwoWay);

    public static ICommand? GetLostFocusCommand(Entry entry) =>
        (ICommand?)entry.GetValue(LostFocusCommandProperty);

    public static void SetLostFocusCommand(Entry entry, ICommand value) =>
        entry.SetValue(LostFocusCommandProperty, value);
    
    #endregion
    
    protected override void OnAttachedTo(Entry bindable)
    {
        base.OnAttachedTo(bindable);
        bindable.Unfocused += Entry_Unfocused;
    }

    protected override void OnDetachingFrom(Entry bindable) 
    {
        base.OnDetachingFrom(bindable);
        bindable.Unfocused -= Entry_Unfocused;
    }

    private void Entry_Unfocused(object? sender, FocusEventArgs e)
    {
        if (sender is not Entry entry) return;
        
        SetText(entry, entry.Text);
        GetLostFocusCommand(entry)?.Execute(null);
    }
}