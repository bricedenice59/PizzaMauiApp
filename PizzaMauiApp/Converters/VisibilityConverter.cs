using System.Globalization;

namespace PizzaMauiApp.Converters;

public class VisibilityConverter : IValueConverter
{
    public Visibility WhenTrue { get; set; } = Visibility.Visible;
    public Visibility WhenFalse { get; set; } = Visibility.Collapsed;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var flag = value as bool? ?? false;
            
        return flag ? WhenTrue : WhenFalse;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var vis = value as Visibility? ?? WhenFalse;

        return vis == WhenTrue;
    }
    
}