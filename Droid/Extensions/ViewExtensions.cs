﻿using Android.Graphics.Drawables;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;
using Android.OS;

namespace Mandrilkalender.Droid
{
	public static class UIViewExtensions
	{
		public static void InitializeFrom(this View nativeControl, RoundedBoxView formsControl)
		{
			if (nativeControl == null || formsControl == null)
				return;

			var background = new GradientDrawable();

			background.SetColor(formsControl.BackgroundColor.ToAndroid());

			if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
			{
				nativeControl.Background = background;
			}
			else {
				nativeControl.SetBackgroundDrawable(background);
			}

			nativeControl.UpdateCornerRadius(formsControl.CornerRadius);
			nativeControl.UpdateBorder(formsControl.BorderColor, formsControl.BorderThickness);
		}

		public static void UpdateFrom(this View nativeControl, RoundedBoxView formsControl,
		  string propertyChanged)
		{
			if (nativeControl == null || formsControl == null)
				return;

			if (propertyChanged == RoundedBoxView.CornerRadiusProperty.PropertyName)
			{
				nativeControl.UpdateCornerRadius(formsControl.CornerRadius);
			}
			if (propertyChanged == VisualElement.BackgroundColorProperty.PropertyName)
			{
				var background = nativeControl.Background as GradientDrawable;

				if (background != null)
				{
					background.SetColor(formsControl.BackgroundColor.ToAndroid());
				}
			}

			if (propertyChanged == RoundedBoxView.BorderColorProperty.PropertyName)
			{
				nativeControl.UpdateBorder(formsControl.BorderColor, formsControl.BorderThickness);
			}

			if (propertyChanged == RoundedBoxView.BorderThicknessProperty.PropertyName)
			{
				nativeControl.UpdateBorder(formsControl.BorderColor, formsControl.BorderThickness);
			}
		}

		public static void UpdateBorder(this View nativeControl, Color color, int thickness)
		{
			var backgroundGradient = nativeControl.Background as GradientDrawable;

			if (backgroundGradient != null)
			{
				var relativeBorderThickness = thickness * 3;
				backgroundGradient.SetStroke(relativeBorderThickness, color.ToAndroid());
			}
		}

		public static void UpdateCornerRadius(this View nativeControl, double cornerRadius)
		{
			var backgroundGradient = nativeControl.Background as GradientDrawable;

			if (backgroundGradient != null)
			{
				var relativeCornerRadius = (float)(cornerRadius * 3.7);
				backgroundGradient.SetCornerRadius(relativeCornerRadius);
			}
		}
	}
}
