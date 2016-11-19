using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace Mandrilkalender.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			//Set static double values in our app to size elements off of screen size
			//Important to note that iOS Forms utilizes real pixels to size elements 
			//This is why we can just grab the pixels of the screen
			App.ScreenWidth = (double)UIScreen.MainScreen.Bounds.Width;
			App.ScreenHeight = (double)UIScreen.MainScreen.Bounds.Height;

			//Set our iOS Device so we can utilize it in our CustomRenderer for screen orientation
			DeviceHelper.iOSDevice = UIDevice.CurrentDevice;

			// Set statusbar to light style - Resulting in white text in the statusbar
			UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);

			LoadApplication(new App());

			// Load themes
			var x = typeof(Xamarin.Forms.Themes.LightThemeResources);
			x = typeof(Xamarin.Forms.Themes.iOS.UnderlineEffect);

			// Ask the user to grant us permissions to display notifications
			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);
				UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
			}

			// check for a notification
			if (options != null)
			{
				// check for a local notification
				if (options.ContainsKey(UIApplication.LaunchOptionsLocalNotificationKey))
				{
					var localNotification = options[UIApplication.LaunchOptionsLocalNotificationKey] as UILocalNotification;
					if (localNotification != null)
					{
						UIAlertController okayAlertController = UIAlertController.Create(localNotification.AlertAction, localNotification.AlertBody, UIAlertControllerStyle.Alert);
						okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

						UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(okayAlertController, true, null);

						// reset our badge
						UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
					}
				}
			}

			return base.FinishedLaunching(app, options);
		}

		public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
		{
			// show an alert
			UIAlertController okayAlertController = UIAlertController.Create(notification.AlertAction, notification.AlertBody, UIAlertControllerStyle.Alert);
			okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

			UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(okayAlertController, true, null);

			// reset our badge
			UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
		}
	}
}
