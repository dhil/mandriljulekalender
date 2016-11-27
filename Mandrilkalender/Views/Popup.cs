using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mandrilkalender
{
	public class Popup : AbsoluteLayout
	{
		public bool IsShown { get; set; }

		private AbsoluteLayout parent;

		public event Play OnPlay;
		public EventArgs e = null;
		public delegate void Play(Gate g, EventArgs e);

		public Popup(AbsoluteLayout parent, Gate gate)
		{
			this.parent = parent;

			// Set default opacity to 0
			Opacity = 0;

			var transparent = Color.Black.MultiplyAlpha(0.6);
			BackgroundColor = transparent;
			AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);

			var image = new Image
			{
				Source = GateService.GetImageName(gate),
				Aspect = Aspect.AspectFit,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			var captionText = new Label
			{
				Text = $"{gate.Number}. december",
				HorizontalOptions = LayoutOptions.Fill,
				StyleClass = new List<string>() { "Header" }
			};
			var descriptionText = new Label
			{
				Text = gate.Description,
				HorizontalOptions = LayoutOptions.Fill,
			};

			var playBtn = new Button
			{
				Text = "Afspil",
				VerticalOptions = LayoutOptions.End,
				StyleClass = new List<string>() { "Success" }
			};
			playBtn.Clicked += (sender, e) =>
			{
				if (OnPlay != null)
				{
					OnPlay(gate, null);
				}
			};

			var layout = new Grid
			{
				Padding = new Thickness(10, 10, 10, 10),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};
			layout.RowDefinitions = new RowDefinitionCollection();
			layout.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
			layout.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
			layout.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
			layout.ColumnDefinitions = new ColumnDefinitionCollection();
			layout.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
			layout.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
			layout.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
			layout.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

			if (Device.OS != TargetPlatform.Android)
			{
				var cancelBtn = new Button
				{
					Text = "Annuller",
					VerticalOptions = LayoutOptions.End,
					StyleClass = new List<string>() { "Danger" }
				};
				cancelBtn.Clicked += (sender, e) =>
				{
					Hide();
				};
				layout.Children.Add(playBtn, 2, 2);
				Grid.SetColumnSpan(playBtn, 2);
				layout.Children.Add(cancelBtn, 0, 2);
				Grid.SetColumnSpan(cancelBtn, 2);
			}
			else {
				layout.Children.Add(playBtn, 0, 2);
				Grid.SetColumnSpan(playBtn, 4);
			}

			layout.Children.Add(image, 0, 0);
			Grid.SetRowSpan(image, 2);

			layout.Children.Add(captionText, 1, 0);
			Grid.SetColumnSpan(captionText, 3);
			layout.Children.Add(descriptionText, 1, 1);
			Grid.SetColumnSpan(descriptionText, 3);

			var wrapper = new Grid()
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
			};
			wrapper.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
			wrapper.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

			var background = new RoundedBoxView
			{
				BackgroundColor = Color.FromHex("#F5F5F5"),
				CornerRadius = 5,
				HorizontalOptions = LayoutOptions.Fill,
				VerticalOptions = LayoutOptions.Fill,
			};
			wrapper.Children.Add(background, 0, 0);

			wrapper.Children.Add(layout, 0, 0);

			var contentView = new ContentView()
			{
				Content = wrapper
			};
			Children.Add(contentView, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);

			// Gesture recognizers.
			// Ensure that taps within the actual box does not hide the popup
			background.GestureRecognizers.Add(new TapGestureRecognizer()
			{
				Command = new Command((obj) => DoNothing())
			});
			// Taps outside the "popup" layout will hide it.
			GestureRecognizers.Add(new TapGestureRecognizer()
			{
				Command = new Command((obj) => Hide())
			});
		}

		public void DoNothing() { }

		public void Show()
		{
			parent.Children.Add(this);
			IsShown = true;

			// Fade to opacity 100%
			this.FadeTo(1, 500);
		}

		public void Hide()
		{
			//await this.FadeTo(0, 500);
			parent.Children.Remove(this);
			IsShown = false;
		}
	}
}
