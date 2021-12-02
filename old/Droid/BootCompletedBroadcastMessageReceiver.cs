using System;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Widget;

namespace Mandrilkalender.Droid
{
	[BroadcastReceiver]
	[IntentFilter(new[] { Intent.ActionBootCompleted })]
	public class BootCompletedBroadcastMessageReceiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			if (intent.Action == Intent.ActionBootCompleted)
			{
				Log.Debug("Mandril", "Boot receiver hit! MANDRIL!!");

				NotificationHelper.ScheduleNotifications();
			}
		}
	}
}
