using System;
using Android.App;
using Android.Views;

namespace Mandrilkalender.Droid
{
	//Create static values for the ActionBar and DecorView
	//These will be utilized to hide the notification bar and ActionBar for fullscreen
	public static class StatusBarHelper
	{
		public static View DecorView
		{
			get;
			set;
		}
		public static ActionBar AppActionBar
		{
			get;
			set;
		}
	}
}
