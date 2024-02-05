namespace PizzaMauiApp.Controls;

public partial class RatingControl
{
    #region MyRegion BindableProperties

    public static readonly BindableProperty OnSelectedBackgroundColorProperty = BindableProperty.Create(nameof(OnSelectedBackgroundColor), typeof(Color), typeof(RatingControl), Colors.Yellow);
    public static readonly BindableProperty DefaultBackgroundColorProperty = BindableProperty.Create(nameof(DefaultBackgroundColor), typeof(Color), typeof(RatingControl), Colors.White);

    public static readonly BindableProperty ScaleToProperty = BindableProperty.Create(nameof(ScaleTo), typeof(double), typeof(RatingControl), 1.0);
    public static readonly BindableProperty ScaleAnimationDelayProperty = BindableProperty.Create(nameof(ScaleAnimationDelay), typeof(uint), typeof(RatingControl), 250U);

    public static readonly BindableProperty MaxRatingValueProperty = BindableProperty.Create(nameof(MaxRatingValue), typeof(int), typeof(RatingControl), 1);
    public static readonly BindableProperty RatingValueProperty = BindableProperty.Create(nameof(RatingValue), typeof(int), typeof(RatingControl), 0, BindingMode.TwoWay);

    public static readonly BindableProperty StarWidthRequestProperty = BindableProperty.Create(nameof(StarWidthRequest), typeof(double), typeof(RatingControl), 20.0);
    public static readonly BindableProperty StarHeightRequestProperty = BindableProperty.Create(nameof(StarHeightRequest), typeof(double), typeof(RatingControl), 20.0);

    public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(RatingControl));
    
    private const string StarButtonName = "sb_";
    private Dictionary<int, StarButton> _dicStarButton = new ();
    
    public Color DefaultBackgroundColor
    {
        get => (Color)GetValue(DefaultBackgroundColorProperty);
        set => SetValue(DefaultBackgroundColorProperty, value);
    }
    
    public Color OnSelectedBackgroundColor
    {
        get => (Color)GetValue(OnSelectedBackgroundColorProperty);
        set => SetValue(OnSelectedBackgroundColorProperty, value);
    }
    public double ScaleTo
    {
        get => (double)GetValue(ScaleToProperty);
        set => SetValue(ScaleToProperty, value);
    }
    
    public uint ScaleAnimationDelay
    {
        get => (uint)GetValue(ScaleAnimationDelayProperty);
        set => SetValue(ScaleAnimationDelayProperty, value);
    }
    
    public int MaxRatingValue
    {
        get => (int)GetValue(MaxRatingValueProperty);
        set => SetValue(MaxRatingValueProperty, value);
    }
    
    public int RatingValue
    {
        get => (int)GetValue(RatingValueProperty);
        set => SetValue(RatingValueProperty, value);
    }
    
    public double StarWidthRequest
    {
        get => (double)GetValue(StarWidthRequestProperty);
        set => SetValue(StarWidthRequestProperty, value);
    }

    public double StarHeightRequest
    {
        get => (double)GetValue(StarHeightRequestProperty);
        set=> SetValue(StarHeightRequestProperty, value); 
    }
    
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    #endregion

    #region Ctor
    
    public RatingControl()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }
    
    #endregion
    
    #region Methods
    
    private async void OnLoaded(object? sender, EventArgs e)
    {
        for (int i = 0; i < MaxRatingValue; i++)
        {
            var component = CreateRatingComponent(i);
            
            _dicStarButton.Add(i, component);
            StackLayout.Add(component);
        }
        
        // bind value from viewmodel => control
        if (RatingValue != 0)
        {
            for (int i = 0; i < RatingValue; i++)
            {
                await Select(i);
            }
        }
    }

    private StarButton CreateRatingComponent(int i)
    {
        var starButton = new StarButton
        {
            Name = $"{StarButtonName}{i}",
            ShapeBackgroundColor = DefaultBackgroundColor,
            StrokeThickness = 0,
            WidthRequest = StarWidthRequest,
            HeightRequest = StarHeightRequest
        };
        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += OnTapped;
        starButton.GestureRecognizers.Add(tapGestureRecognizer);

        return starButton;
    }

    private async void OnTapped(object? sender, TappedEventArgs e)
    {
        if (IsReadOnly) return;
        
        if (sender is null) return;
        if (sender is StarButton starButton)
        {
            var selectedStarIndexStr = starButton.Name.Substring(StarButtonName.Length);
            if (!int.TryParse(selectedStarIndexStr, out int selectedStarIndex)) return;
            for (int i = 0; i <= selectedStarIndex; i++)
            {
                await Select(i);
            }
            for (int j = selectedStarIndex + 1; j < _dicStarButton.Count; j++)
            {
                if (!_dicStarButton[j].ShapeBackgroundColor.Equals(DefaultBackgroundColor))
                    UnSelect(j);
            }

            RatingValue = selectedStarIndex;
        }
    }

    private async Task Select(int toSelect)
    {
        await _dicStarButton[toSelect].ScaleTo(ScaleTo, ScaleAnimationDelay);
        _dicStarButton[toSelect].ShapeBackgroundColor = OnSelectedBackgroundColor;
        await _dicStarButton[toSelect].ScaleTo(1, ScaleAnimationDelay);
    }
    
    private void UnSelect(int toUnselect)
    {
        _dicStarButton[toUnselect].ShapeBackgroundColor = DefaultBackgroundColor;
    }
    
    #endregion
}