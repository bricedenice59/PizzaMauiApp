using System.Windows.Input;

namespace PizzaMauiApp.Controls;

public partial class EntryWithIconControl
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(PasswordBox), null);
    
    public static readonly BindableProperty IconSourceProperty =
        BindableProperty.Create(nameof(IconSource), typeof(string), typeof(PasswordBox), null);

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(PasswordBox), null);

    public static readonly BindableProperty IconHeightProperty =
        BindableProperty.Create(nameof(IconHeight), typeof(double), typeof(PasswordBox), 0.0);
    
    public static readonly BindableProperty IconWidthProperty =
        BindableProperty.Create(nameof(IconWidth), typeof(double), typeof(PasswordBox), 0.0);

    public static readonly BindableProperty UnfocusedCommandProperty =
        BindableProperty.Create(nameof(UnfocusedCommand), typeof(ICommand), typeof(PasswordBox), null);
    
    public static readonly BindableProperty IsValidProperty =
        BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(PasswordBox), false);

    
    public string? Text
    {
        get => (string?)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    
    public string? IconSource
    {
        get => (string?)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }
    
    public string? Placeholder
    {
        get => (string?)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }
    
    public double IconHeight
    {
        get => (double)GetValue(IconHeightProperty);
        set => SetValue(IconHeightProperty, value);
    }
    
    public double IconWidth
    {
        get => (double)GetValue(IconWidthProperty);
        set => SetValue(IconWidthProperty, value);
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
    
    public EntryWithIconControl()
    {
        InitializeComponent();
    }
}