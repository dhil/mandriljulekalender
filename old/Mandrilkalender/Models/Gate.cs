using System;
using Realms;

namespace Mandrilkalender
{
	public class Gate : RealmObject
	{

		public int X { get; set; }
		public int Y { get; set; }
		public int Number { get; set; }
		public bool Opened { get; set; }
		public string Description { get; set; }
		public string VideoId { get; set; }

		public Gate() 
		{
		}

		public Gate(int number, int x, int y, string description, string videoId) : this(number, x, y, description, videoId, false)
		{
		}

		public Gate(int number, int x, int y, string description, string videoId, bool opened) : this()
		{
			Number = number;
			X = x;
			Y = y;
			Opened = opened;
			Description = description;
			VideoId = videoId;
		}

		public string GetYoutubeLink()
		{
			return $"https://www.youtube.com/watch?v={VideoId}";
		}
	}
}
