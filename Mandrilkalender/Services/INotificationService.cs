using System;
namespace Mandrilkalender
{
	public interface INotificationService
	{
		void ScheduleNotification(string title, string Content, DateTime time);
	}
}
