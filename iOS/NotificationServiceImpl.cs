using System;
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
			UILocalNotification notification = new UILocalNotification();

			notification.AlertAction = "Vis";
			notification.AlertBody = $"{title}\n{body}";
			notification.ApplicationIconBadgeNumber = UIApplication.SharedApplication.ApplicationIconBadgeNumber + 1;
			notification.FireDate = time.ToNSDate();
			UIApplication.SharedApplication.ScheduleLocalNotification(notification);
		}
	}
}
