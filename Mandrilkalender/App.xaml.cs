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

			// TODO: Schedule notificaitonss
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
	}
}
