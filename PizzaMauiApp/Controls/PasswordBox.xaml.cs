namespace PizzaMauiApp.Controls;

public partial class PasswordBox
{
    public static readonly BindableProperty PasswordProperty =
        BindableProperty.Create(nameof(Password), typeof(string), typeof(PasswordBox), null);
    
    public string? Password
    {
        get => (string?)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
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