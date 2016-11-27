using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Mandrilkalender
{
	public class GateControl : AbsoluteLayout
	{
		private static Color BlueColor = (Color) App.Current.Resources["mandrilBlue"];

		private Gate gate;
		private ViewFlipper flipper;
		private Label number;

		public event ClickHandler GateClicked;
		public EventArgs e = null;
		public delegate void ClickHandler(Gate g, EventArgs e);

		public GateControl(Gate gate)
		{
			this.gate = gate;

			BackgroundColor = BlueColor;

			var frontView = new Image()
			{
				Source = "gate.png",
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
				Aspect = Aspect.AspectFill
			};

			var backView = new Image()
			{
				Source = GateService.GetImageName(gate),
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center,
				Aspect = Aspect.AspectFill
			};

			flipper = new ViewFlipper(RequestFlip)
			{
				FrontView = frontView,
				BackView = backView,
				RotationDirection = RotationDirection.Horizontal
			};

			if (gate.Opened)
			{
				flipper.TriggerEventOnFlip = false;
				flipper.FlipOnTap = false;
				flipper.FlipState = FlipState.Back;
			}

			flipper.OnFlipped += (flipper, e) => OnTapped();

			AbsoluteLayout.SetLayoutBounds(flipper, new Rectangle(0.5, 0.5, -1, -1));
			AbsoluteLayout.SetLayoutFlags(flipper, AbsoluteLayoutFlags.PositionProportional);

			number = new Label()
			{
				TextColor = Color.White,
				Text = gate.Number.ToString()
			};

			AbsoluteLayout.SetLayoutBounds(number, new Rectangle(0.95, 0, -1, -1));
			AbsoluteLayout.SetLayoutFlags(number, AbsoluteLayoutFlags.PositionProportional);

			GestureRecognizers.Add(new TapGestureRecognizer()
			{
				Command = new Command((obj) =>
				{
					if (!RequestFlip())
					{
						return;
					}

					if (flipper.FlipState == FlipState.Front)
					{
						flipper.FlipState = FlipState.Back;
					}
					else {
						OnTapped();
					}
				})
			});

			Children.Add(flipper);
			Children.Add(number);

		}

		public bool RequestFlip()
		{
			var now = DateTime.Now.Date;
			if (now.Month == 12 && now.Day >= gate.Number)
			{
				return true;
			}

			DependencyService.Get<IToastService>().ShowToast($"Vitter Rynkemås mener ikke det er den {gate.Number}. december endnu!", 3000);

			return false;
		}

		private void OnTapped()
		{
			App.GateService.OpenGate(this.gate);

			flipper.FlipOnTap = false;

			if (GateClicked != null)
			{
				GateClicked(gate, e);
			}
		}
	}
}
