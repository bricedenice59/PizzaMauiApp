using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;

namespace PizzaMauiApp.Droid.Renderers;

internal class CustomShellHandler : ShellRenderer
{
	protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
	{
		return new CustomShellBottomNavViewAppearanceTracker(this, shellItem.CurrentItem);
	}
}