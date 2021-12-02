using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Java.Text;
using Java.Util;
using Mandrilkalender.Droid;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationServiceImpl))]
namespace Mandrilkalender.Droid
{
	public class NotificationServiceImpl : INotificationService
	{
		public void ScheduleNotification(DateTime dateTime)
		{
			Intent alarmIntent = new Intent(Forms.Context, typeof(AlarmReceiver));
			alarmIntent.PutExtra("title", $"Glædelig {dateTime.Day}. december");
			alarmIntent.PutExtra("message", $"Hvad mon venter bag låge nummer {dateTime.Day}?");
			alarmIntent.PutExtra("id", dateTime.Day);

			PendingIntent pendingIntent = PendingIntent.GetBroadcast(Forms.Context, dateTime.Day, alarmIntent, PendingIntentFlags.UpdateCurrent);
			AlarmManager alarmManager = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);

			// Convert DateTime to Java Calendar object
			var cal = Calendar.Instance;
			cal.Set(dateTime.Year, dateTime.Month-1, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);

			alarmManager.Set(AlarmType.Rtc, cal.TimeInMillis, pendingIntent);

			Log.Debug("MANDRILTIME", $"Alarm scheduled at {SimpleDateFormat.Instance.Format(cal.Time)}");
		}
	}
}
