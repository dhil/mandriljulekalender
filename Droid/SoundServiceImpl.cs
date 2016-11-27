using System;
using Android.Media;
using Mandrilkalender.Droid;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(SoundServiceImpl))]
namespace Mandrilkalender.Droid
{
	public class SoundServiceImpl : ISoundService
	{
		public void PlayIntro()
		{
			AudioManager manager = (AudioManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.AudioService);

			if (!manager.IsMusicActive)
			{
				var mp = MediaPlayer.Create(Forms.Context, Android.Net.Uri.Parse($"android.resource://{Forms.Context.ApplicationContext.PackageName}/raw/" + Resource.Raw.intro));
				mp.Start();
			}
		}
	}
}
