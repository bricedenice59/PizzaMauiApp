namespace PizzaMauiApp.Controls;

public partial class StarButton
{ 
    public static readonly BindableProperty BorderBackgroundColorProperty = BindableProperty.Create(nameof(BorderBackgroundColor), typeof(Color), typeof(StarButton), Colors.Aqua);
    public static readonly BindableProperty BorderStrokeProperty = BindableProperty.Create(nameof(BorderStroke), typeof(Brush), typeof(StarButton), Brush.Aqua);
    public static readonly BindableProperty BorderStrokeThicknessProperty = BindableProperty.Create(nameof(BorderStrokeThickness), typeof(string), typeof(StarButton), string.Empty);

    public Color BorderBackgroundColor
    {
        get => (Color)GetValue(BorderBackgroundColorProperty);
        set => SetValue(BorderBackgroundColorProperty, value);
    }
    public Brush BorderStroke
    {
        get => (Brush)GetValue(BorderStrokeProperty);
        set => SetValue(BorderStrokeProperty, value);
    }
    public string BorderStrokeThickness
    {
        get => (string)GetValue(BorderStrokeThicknessProperty);
        set => SetValue(BorderStrokeThicknessProperty, value);
    }
    
    public StarButton()
    {
        InitializeComponent();
    }
}