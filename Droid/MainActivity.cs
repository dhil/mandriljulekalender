using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Mandrilkalender.Droid
{

	[Activity(Label = "Mandrilkalender", MainLauncher = true, HardwareAccelerated = true, Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			global::Xamarin.Forms.Forms.Init(this, bundle);

			//Calculate the pixes and pass them to our static application doubles
			//We need to make sure we are using device independent pixels (DIP)
			//All Android Forms sizing requests utilize DIPs, so we need that here
			var pixelWidth = (int)Resources.DisplayMetrics.WidthPixels;
			var pixelHeight = (int)Resources.DisplayMetrics.HeightPixels;
			var screenPixelDensity = (double)Resources.DisplayMetrics.Density;

			App.ScreenHeight = (double)((pixelHeight - 0.5f) / screenPixelDensity);
			App.ScreenWidth = (double)((pixelWidth - 0.5f) / screenPixelDensity);


			//Set our status bar helper DecorView. This enables us to hide the notification bar for fullscreen
			StatusBarHelper.DecorView = this.Window.DecorView;

			LoadApplication(new App());

			// load light theme
			var x = typeof(Xamarin.Forms.Themes.LightThemeResources);
			x = typeof(Xamarin.Forms.Themes.Android.UnderlineEffect);
		}

		public override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();
			//Set our status bar helper AppActionBar. This enables us to hide the ActionBar for fullscreen
			//Always hide the actionbar/toolbar if you are hiding the notification bar
			if (ActionBar != null)
				StatusBarHelper.AppActionBar = ActionBar;
		}
	}
}
