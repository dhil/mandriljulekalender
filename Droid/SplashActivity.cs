
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Xamarin.Forms.Platform.Android;

namespace Mandrilkalender.Droid
{
	[Activity(Label = "Mandrilkalender", Theme = "@style/MyTheme.Splash", NoHistory = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class SplashActivity : Activity
	{
		static readonly string TAG = "X:" + typeof(SplashActivity).Name;

		public override void OnCreate(Bundle bundle, PersistableBundle persistentState)
		{
			base.OnCreate(bundle, persistentState);

			Log.Debug(TAG, "SplashActivity.OnCreate");

		}

		protected override void OnResume()
		{
			base.OnResume();

			var intent = new Intent(this, typeof(MainActivity));
			StartActivity(intent);
		}
	}
}
