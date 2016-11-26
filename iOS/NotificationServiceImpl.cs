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
		private bool IsNotificationScheduled(DateTime date)
		{
			var notifications = UIApplication.SharedApplication.ScheduledLocalNotifications;

			foreach (var notification in notifications)
			{
				if (notification.FireDate.ToDateTime().Date == date.Date)
				{
					return true;
				}
			}

			return false;
		}

		public void ScheduleNotification(DateTime dateTime)
		{
			if (IsNotificationScheduled(dateTime))
			{
				return;
			}

			var title = $"Glædelig {dateTime.Day}. december";
			var content = $"Hvad mon venter bag låge nummer {dateTime.Day}?";
			UILocalNotification notification = new UILocalNotification()
			{
				AlertAction = "Vis",
				AlertTitle = title,
				AlertBody = $"{content}",
				ApplicationIconBadgeNumber = 1,
				FireDate = dateTime.ToNSDate(),
				SoundName = "intro.caf"
			};

			UIApplication.SharedApplication.ScheduleLocalNotification(notification);
		}
	}
}
