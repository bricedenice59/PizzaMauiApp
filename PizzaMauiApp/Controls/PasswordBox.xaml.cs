using System.Windows.Input;

namespace PizzaMauiApp.Controls;

public partial class PasswordBox
{
    public static readonly BindableProperty PasswordProperty =
        BindableProperty.Create(nameof(Password), typeof(string), typeof(PasswordBox), null);
    
    public static readonly BindableProperty UnfocusedCommandProperty =
        BindableProperty.Create(nameof(UnfocusedCommand), typeof(ICommand), typeof(PasswordBox), null);
    
    public static readonly BindableProperty IsValidProperty =
        BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(PasswordBox), false);

    public string? Password
    {
        get => (string?)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }
    
    public ICommand? UnfocusedCommand
    {
        get => (ICommand?)GetValue(UnfocusedCommandProperty);
        set => SetValue(UnfocusedCommandProperty, value);
    }
    
    public bool IsValid
    {
        get => (bool)GetValue(IsValidProperty);
        set => SetValue(IsValidProperty, value);
    }
    
    public PasswordBox()
    {
        InitializeComponent();
    }

    private void Entry_OnTapped(object? sender, TappedEventArgs e)
    {
        entry.IsPassword = !entry.IsPassword;
    }
}