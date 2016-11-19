
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Mandrilkalender.Droid
{
	[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
	public class SplashActivity : AppCompatActivity
	{
		static readonly string TAG = "X:" + typeof(SplashActivity).Name;
		private Bundle bundle;
		public override void OnCreate(Bundle bundle, PersistableBundle persistentState)
		{
			base.OnCreate(bundle, persistentState);

			this.bundle = bundle;

			Log.Debug(TAG, "SplashActivity.OnCreate");
		}

		protected override void OnResume()
		{
			base.OnResume();

			Task startupWork = new Task(() =>
			{
				Log.Debug(TAG, "Performing some startup work that takes a bit of time.");

				global::Xamarin.Forms.Forms.Init(this, bundle);

				//Calculate the pixes and pass them to our static application doubles
				//We need to make sure we are using device independent pixels (DIP)
				//All Android Forms sizing requests utilize DIPs, so we need that here
				var pixelWidth = (int)Resources.DisplayMetrics.WidthPixels;
				var pixelHeight = (int)Resources.DisplayMetrics.HeightPixels;
				var screenPixelDensity = (double)Resources.DisplayMetrics.Density;

				App.ScreenHeight = (double)((pixelHeight - 0.5f) / screenPixelDensity);
				App.ScreenWidth = (double)((pixelWidth - 0.5f) / screenPixelDensity);

				Log.Debug(TAG, "Working in the background - important stuff.");
			});

			startupWork.ContinueWith(t =>
			{
				Log.Debug(TAG, "Work is finished - start MainActivity.");
				StartActivity(new Intent(Application.Context, typeof(MainActivity)));
			}, TaskScheduler.FromCurrentSynchronizationContext());

			startupWork.Start();
		}
	}
}
