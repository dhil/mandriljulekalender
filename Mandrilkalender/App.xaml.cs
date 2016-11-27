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

			NotificationHelper.ScheduleNotifications();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
			DependencyService.Get<ISoundService>().PlayIntro();
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

		private static string GetNotificationText(int day)
		{
			return $"Hvad mon venter bag låge nummer {day}?";
		}
	}
}
