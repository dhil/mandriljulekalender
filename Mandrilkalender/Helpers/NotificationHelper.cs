using System;
using Xamarin.Forms;

namespace Mandrilkalender
{
	public static class NotificationHelper
	{
		public static DateTime FROM = new DateTime(DateTime.Now.Year, 11, 1);
		public static DateTime THROUGH = new DateTime(DateTime.Now.Year, 11, 30);
		public static TimeSpan TRIGGER_TIME = new TimeSpan(15, 30, 0);

		public static void ScheduleNotifications()
		{
			var from = FROM;
			var now = DateTime.Now;

			if (now.Month == 11)
			{
				from = now.Date;
			}

			while (from.Date <= THROUGH.Date)
			{
				if (now.TimeOfDay <= TRIGGER_TIME)
				{
					var date = from.Date + TRIGGER_TIME;

					DependencyService.Get<INotificationService>().ScheduleNotification(date);
				}

				from = from.AddDays(1);
			}
		}
	}
}
