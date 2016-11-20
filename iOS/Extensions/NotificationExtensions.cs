using System;
using System.Diagnostics.Contracts;
using UIKit;

namespace Mandrilkalender.iOS
{
	public static class NotificationExtensions
	{
		public static MandrilNotification ToMandrilNotification(this UILocalNotification notification)
		{
			return new MandrilNotification
			{
				Title = notification.AlertTitle,
				Content = notification.AlertBody,
				FireDate = notification.FireDate.ToDateTime()
			};
		}
	}
}
