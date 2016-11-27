using System;
using Android.Media;
using Android.Widget;
using Mandrilkalender.Droid;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(ToastServiceImpl))]
namespace Mandrilkalender.Droid
{
	public class ToastServiceImpl : IToastService
	{
		private Toast toast;

		public void ShowToast(string message, int duration)
		{
			if (toast != null)
			{
				toast.Cancel();
			}

			toast = Toast.MakeText(Forms.Context, message, ToastLength.Long);
			toast.Show();
		}
	}
}
