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
	[Activity(Label = "Mandrilkalender", HardwareAccelerated = true, Icon = "@drawable/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

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
