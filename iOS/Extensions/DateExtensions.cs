using System;
using Foundation;

namespace Mandrilkalender.iOS
{
	public static class DateExtensions
	{
		public static DateTime ToDateTime(this NSDate date)
		{
			return ((DateTime)date).ToLocalTime();
		}

		public static NSDate ToNSDate(this DateTime date)
		{
			if (date.Kind == DateTimeKind.Unspecified)
				date = DateTime.SpecifyKind(date, DateTimeKind.Local);

			return (NSDate)date;
		}
	}
}
