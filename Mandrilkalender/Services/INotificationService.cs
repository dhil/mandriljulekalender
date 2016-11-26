using System;
using System.Collections.Generic;

namespace Mandrilkalender
{
	public interface INotificationService
	{
		void ScheduleNotification(DateTime time);
	}
}
