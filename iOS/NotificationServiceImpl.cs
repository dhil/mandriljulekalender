using System;
using System.Collections.Generic;
using Foundation;
using Mandrilkalender.iOS;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationServiceImpl))]
namespace Mandrilkalender.iOS
{
	public class NotificationServiceImpl : INotificationService
	{
		public void ScheduleNotification(string title, string body, DateTime time)
		{
			UILocalNotification notification = new UILocalNotification()
			{
				AlertAction = "Vis",
				AlertTitle = title,
				AlertBody = $"{title}\n{body}",
				ApplicationIconBadgeNumber = 1,
				FireDate = time.ToNSDate()
			};

			UIApplication.SharedApplication.ScheduleLocalNotification(notification);
		}

		public List<MandrilNotification> GetScheduledNotifications()
		{
			var notifications = UIApplication.SharedApplication.ScheduledLocalNotifications;

			var scheduledNotifications = new List<MandrilNotification>();

			foreach (var notification in notifications)
			{
				scheduledNotifications.Add(notification.ToMandrilNotification());
			}

			return scheduledNotifications;
		}

		public bool IsNotificationScheduled(DateTime date)
		{
			var notifications = UIApplication.SharedApplication.ScheduledLocalNotifications;
			var scheduledNotifications = new List<MandrilNotification>();

			foreach (var notification in notifications)
			{
				if (notification.FireDate.ToDateTime().Date == date.Date)
				{
					return true;
				}
			}

			return false;
		}
	}
}
