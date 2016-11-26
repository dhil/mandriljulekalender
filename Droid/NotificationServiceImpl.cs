using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Support.V7.App;
using Mandrilkalender.Droid;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationServiceImpl))]
namespace Mandrilkalender.Droid
{
	public class NotificationServiceImpl : INotificationService
	{
		public List<MandrilNotification> GetScheduledNotifications()
		{
			throw new NotImplementedException();
		}

		public bool IsNotificationScheduled(DateTime date)
		{
			throw new NotImplementedException();
		}

		public void ScheduleNotification(string title, string content, DateTime time)
		{
			var notification = CreateNotification(title, content);
		}

		private Notification CreateNotification(string title, string content)
		{
			// Set up an intent so that tapping the notifications returns to this app:
			Intent intent = new Intent(Forms.Context, typeof(MainActivity));

			// Create a PendingIntent; we're only using one PendingIntent (ID = 0):
			const int pendingIntentId = 0;
			PendingIntent pendingIntent =
				PendingIntent.GetActivity(Forms.Context, pendingIntentId, intent, PendingIntentFlags.OneShot);


			Notification.Builder builder = new Notification.Builder(Forms.Context)
				.SetContentIntent(pendingIntent)
				.SetSmallIcon(Resource.Drawable.icon)
				.SetContentTitle(title)
				.SetContentText(content)
				.SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
				.SetSound(Android.Net.Uri.Parse($"android.resource://{Forms.Context.ApplicationContext.PackageName}/raw/intro"));
			

			// Build the notification:
			Notification notification = builder.Build();

			return notification;
		}
	}
}
