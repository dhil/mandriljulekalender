using System;
using System.Collections.Generic;

namespace Mandrilkalender
{
	public interface INotificationService
	{
		void ScheduleNotification(string title, string Content, DateTime time);
		List<MandrilNotification> GetScheduledNotifications();
		bool IsNotificationScheduled(DateTime date);
	}
}
