using CoreAnimation;
using CoreGraphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Graphics.Platform;
using UIKit;

namespace PizzaMauiApp.iOS.Renderers;

partial class CustomShellHandler : ShellRenderer
{
    protected override IShellTabBarAppearanceTracker CreateTabBarAppearanceTracker()
	{
		return new CustomShellTabBarAppearanceTracker();
	}
}

public class CustomShellTabBarAppearanceTracker : IShellTabBarAppearanceTracker
{
    public void Dispose()
    {
        // TODO release managed resources here
    }

    public void ResetAppearance(UITabBarController controller)
    {

    }

    public void SetAppearance(UITabBarController controller, ShellAppearance appearance)
    {
#pragma warning disable CA1422
        controller.TabBar.SelectedImageTintColor = UIColor.Blue;
#pragma warning restore CA1422
        controller.TabBar.UnselectedItemTintColor = UIColor.White;
    }

    public void UpdateLayout(UITabBarController controller)
    {
        const int bottomSpace = 0;
        const int margin = 0;
        
        controller.TabBar.Frame = new CGRect(controller.TabBar.Frame.X + margin,
            controller.TabBar.Frame.Y - bottomSpace,
            controller.TabBar.Frame.Width - (2 * margin),
            controller.TabBar.Frame.Height);

        const int cornerRadius = 5;
        var uIBezierPath = UIBezierPath.FromRoundedRect(controller.TabBar.Bounds, UIRectCorner.AllCorners,
            new CGSize(cornerRadius, cornerRadius));

        object? primaryBackgroundColorObj = null;
        Color? backgroundColor = null;
        if (App.Current?.Resources.TryGetValue("NavBarColor", out primaryBackgroundColorObj) == true)
        {
            if (primaryBackgroundColorObj != null)
            {
                var color = (Color)primaryBackgroundColorObj;
                backgroundColor = Color.FromRgb(color.Red, color.Green, color.Blue);
            }
        }
        
        var cAShapeLayer = new CAShapeLayer
        {
            Frame = controller.TabBar.Bounds,
            Path = uIBezierPath.CGPath,
            FillColor = backgroundColor != null 
                ? backgroundColor.AsCGColor()
                : UIColor.White.CGColor
        };

        controller.TabBar.Layer.AddSublayer(cAShapeLayer);
        
        if (controller.TabBar.Items == null) return;
        
        foreach (var tabBarItem in controller.TabBar.Items)
        {
            //define title position and default fontsize
            tabBarItem.TitlePositionAdjustment = new UIOffset(horizontal: 0, vertical: +7);
            tabBarItem.SetTitleTextAttributes(new UIStringAttributes
            {
                Font = UIFont.SystemFontOfSize(12, UIFontWeight.Medium)
            }, UIControlState.Normal);
            
            //resize image
            var prevImage = tabBarItem.Image;
            var size = new CGSize(28, 28);
            var renderer = new UIGraphicsImageRenderer(size, new UIGraphicsImageRendererFormat
            {
                Opaque = false,
                Scale = 0
            });
            var resizedImage = renderer.CreateImage(imageContext =>
            {
                prevImage?.Draw(new CGRect(new CGPoint(0, 0), size));
            });
        
            tabBarItem.Image = resizedImage;
        }
    }
}

