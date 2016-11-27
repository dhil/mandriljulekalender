using System;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Android.Util;
using Xamarin.Forms;

namespace Mandrilkalender.Droid
{
	[BroadcastReceiver]
	public class AlarmReceiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			Log.Debug("MANDRILTIME", $"Alarm receiver!!");
			var message = intent.GetStringExtra("message");
			var title = intent.GetStringExtra("title");
			var id = intent.GetIntExtra("id", 0);

			var notIntent = new Intent(context, typeof(MainActivity));
			notIntent.PutExtra("notificationId", id);
			var contentIntent = PendingIntent.GetActivity(context, 0, notIntent, PendingIntentFlags.CancelCurrent);
			var manager = NotificationManagerCompat.From(context);

			var style = new NotificationCompat.BigTextStyle();
			style.BigText(message);

			NotificationCompat.Builder builder = new NotificationCompat.Builder(Forms.Context)
				.SetContentIntent(contentIntent)
				.SetSmallIcon(Resource.Drawable.icon)
				.SetContentTitle(title)
				.SetContentText(message)
				.SetStyle(style)
				.SetDefaults((int)NotificationDefaults.Vibrate)
				.SetSound(Android.Net.Uri.Parse($"android.resource://{Forms.Context.ApplicationContext.PackageName}/raw/" + Resource.Raw.intro))
				.SetAutoCancel(true);

			// Build the notification:
			Notification notification = builder.Build();

			manager.Notify(id, notification);
		}
	}
}
