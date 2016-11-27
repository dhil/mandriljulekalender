using System;
using Xamarin.Forms;

namespace Mandrilkalender
{
	public static class NotificationHelper
	{
		private static int MONTH = 12;

		public static DateTime FROM = new DateTime(DateTime.Now.Year, MONTH, 1);
		public static DateTime THROUGH = new DateTime(DateTime.Now.Year, MONTH, 25);
		public static TimeSpan TRIGGER_TIME = new TimeSpan(8, 0, 0);

		public static void ScheduleNotifications()
		{
			var from = FROM;
			var now = DateTime.Now;

			if (now.Month == MONTH)
			{
				from = now.Date;
			}

			while (from.Date <= THROUGH.Date)
			{
				if (now.Date < from.Date || (now.Date == from.Date && now.TimeOfDay <= TRIGGER_TIME))
				{
					var date = from.Date + TRIGGER_TIME;

					DependencyService.Get<INotificationService>().ScheduleNotification(date);
				}

				from = from.AddDays(1);
			}
		}
	}
}
