using System;
using AVFoundation;
using Foundation;
using Mandrilkalender.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(SoundServiceImpl))]
namespace Mandrilkalender.iOS
{
	public class SoundServiceImpl : ISoundService
	{
		private AVAudioPlayer backgroundMusic;

		public void PlayIntro()
		{
			// Initialize background music
			var songURL = new NSUrl("intro.caf");
			NSError err;
			backgroundMusic = new AVAudioPlayer(songURL, "caf", out err);
			backgroundMusic.FinishedPlaying += delegate
			{
				backgroundMusic = null;
			};
			backgroundMusic.NumberOfLoops = 0;
			backgroundMusic.Play();
		}
	}
}
