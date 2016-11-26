using System;
using Android.App;
using Android.Content;
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
				Toast.MakeText
				(
					context,
					"The BootCompletedExample application catches the BootCompleted broadcast message",
					ToastLength.Long
				).Show();
			}
		}
	}
}
