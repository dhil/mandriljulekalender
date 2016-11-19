using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Mandrilkalender
{
	public partial class Gates : ContentPage
	{
		private Popup popup;

		public Gates()
		{
			InitializeComponent();

			if (Device.OS == TargetPlatform.iOS)
			{
				Padding = new Thickness(0, 20, 0, 0);
				BackgroundColor = (Color)App.Current.Resources["mandrilBlueDark"];
			}

			var grid = new Grid()
			{
				HorizontalOptions = LayoutOptions.Fill,
				VerticalOptions = LayoutOptions.Fill,
				BackgroundColor = Color.White,
				RowSpacing = 1,
				ColumnSpacing = 1
			};

			grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
			grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
			grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
			grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });
			grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Star });

			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });

			var service = App.GateService;
			var gates = service.GetGates();
			foreach (var gate in gates)
			{
				var gateControl = new GateControl(gate);
				grid.Children.Add(gateControl, gate.X, gate.Y);
				gateControl.GateClicked += GateControl_GateClicked;
			}

			var wrapper = new AbsoluteLayout();
			wrapper.Children.Add(grid);

			AbsoluteLayout.SetLayoutBounds(grid, new Rectangle(0f, 0f, 1, 1));
			AbsoluteLayout.SetLayoutFlags(grid, AbsoluteLayoutFlags.All);

			Content = wrapper;
		}

		void GateControl_GateClicked(Mandrilkalender.Gate g, EventArgs e)
		{
			popup = new Popup(((AbsoluteLayout)Content), g);
			popup.OnPlay += Popup_OnPlay;
			popup.Show();
		}

		async void Popup_OnPlay(Mandrilkalender.Gate g, EventArgs e)
		{
			Device.OpenUri(new Uri($"vnd.youtube://{g.VideoId}"));
			//await Navigation.PushModalAsync(new VideoPage(g), true);
		}

		protected override bool OnBackButtonPressed()
		{
			if (popup != null && popup.IsShown)
			{
				popup.Hide();
				return true;
			}
			return base.OnBackButtonPressed();
		}
	}
}
