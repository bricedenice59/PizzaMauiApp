namespace PizzaMauiApp.Controls;

public partial class StarButton
{ 
    #region BindableProperties
    
    public static readonly BindableProperty ShapeBackgroundColorProperty = BindableProperty.Create(nameof(ShapeBackgroundColor), typeof(Color), typeof(StarButton), Colors.Aqua);
    public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(StarButton), string.Empty);
    
    public Color ShapeBackgroundColor
    {
        get => (Color)GetValue(ShapeBackgroundColorProperty);
        set => SetValue(ShapeBackgroundColorProperty, value);
    }
    
    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }
    
    #endregion

    #region Ctor
    public StarButton()
    {
        InitializeComponent();
    }
    
    #endregion
}