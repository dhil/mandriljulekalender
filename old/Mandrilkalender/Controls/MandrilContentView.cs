using System;
using Xamarin.Forms;

namespace Mandrilkalender
{
	public class MandrilContentView : ContentView
	{
		public Gate Gate { get; private set; }

		public MandrilContentView(Gate g)
		{
			Gate = g;
		}
	}
}
