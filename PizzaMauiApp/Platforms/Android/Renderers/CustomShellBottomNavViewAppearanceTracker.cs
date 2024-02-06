using Android.Graphics.Drawables;
using Android.Views;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;

namespace PizzaMauiApp.Droid.Renderers;
internal class CustomShellBottomNavViewAppearanceTracker : ShellBottomNavViewAppearanceTracker
{
	private readonly IShellContext _shellContext;
	private readonly object? _primaryBackgroundColorObj = null;
    
	public CustomShellBottomNavViewAppearanceTracker(IShellContext shellContext, ShellItem shellItem) : base(
		shellContext, shellItem)
	{
		_shellContext = shellContext;
		App.Current?.Resources.TryGetValue("NavBarColor", out _primaryBackgroundColorObj);
	}

	public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
	{
		base.SetAppearance(bottomView, appearance);
		if (!Shell.GetTabBarIsVisible(_shellContext.Shell.CurrentPage)) return;
		
		var backgroundDrawable = new GradientDrawable();
		backgroundDrawable.SetShape(ShapeType.Rectangle);
		backgroundDrawable.SetCornerRadius(10);

		if (_primaryBackgroundColorObj != null)
		{
			backgroundDrawable.SetColor(((Color)_primaryBackgroundColorObj).ToPlatform());
			bottomView.SetBackground(backgroundDrawable);
		}
		
		var layoutParams = bottomView.LayoutParameters;
		if (layoutParams is ViewGroup.MarginLayoutParams marginLayoutParams)
		{
			const int margin = 0;
			marginLayoutParams.BottomMargin = margin;
			marginLayoutParams.LeftMargin = margin;
			marginLayoutParams.RightMargin = margin;
			bottomView.LayoutParameters = layoutParams;
		}
	}
}