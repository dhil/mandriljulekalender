using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Mandrilkalender
{
	public partial class App : Application
	{
		public static GateService GateService => new GateService();

		//Create two static doubles that will be used to size elements
		public static double ScreenWidth;
		public static double ScreenHeight;

		public App()
		{
			InitializeComponent();

			MainPage = new Gates();

			ScheduleNotifications();

			var scheduledNotifications = DependencyService.Get<INotificationService>().GetScheduledNotifications();
			var i = 1;
			foreach (var n in scheduledNotifications)
			{
				Debug.WriteLine($"{i++}");
				Debug.WriteLine($"{n.Title}");
				Debug.WriteLine($"{n.Content}");
				Debug.WriteLine($"{n.FireDate.ToString("s")}");
				Debug.WriteLine("------\n");
			}
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

		private void ScheduleNotifications()
		{
			var lastDay = 25;
			var now = DateTime.Now;
			var lastDate = new DateTime(now.Year, 12, lastDay, 8, 0, 0);
			var iDate = new DateTime(now.Year, 12, 1, 8, 0, 0);
			var notificationService = DependencyService.Get<INotificationService>();

			if (lastDate < now)
				return;

			for (var i = 1; i <= lastDay; i++)
			{
				if (iDate >= now && !notificationService.IsNotificationScheduled(iDate))
				{
					notificationService.ScheduleNotification(
						$"Glædelig {i}. december",
						GetNotificationText(i),
						iDate);
				}

				iDate = iDate.AddDays(1);
			}
		}

		private static string GetNotificationText(int day)
		{
			return $"Hvad mon venter bag låge nummer {day}?";
		}
	}
}
