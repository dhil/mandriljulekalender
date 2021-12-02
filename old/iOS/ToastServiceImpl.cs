using System;
using Mandrilkalender.iOS;
using ToastIOS;

[assembly: Xamarin.Forms.Dependency(typeof(ToastServiceImpl))]
namespace Mandrilkalender.iOS
{
	public class ToastServiceImpl : IToastService
	{
		public void ShowToast(string message, int duration)
		{
			Toast.MakeText(message, duration).Show();
		}
	}
}
