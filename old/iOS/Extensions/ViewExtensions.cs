﻿using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Mandrilkalender.iOS
{
	public static class UIViewExtensions
	{
		public static void InitializeFrom(this UIView nativeControl, RoundedBoxView formsControl)
		{
			if (nativeControl == null || formsControl == null)
				return;

			nativeControl.Layer.MasksToBounds = true;
			nativeControl.Layer.CornerRadius = (float)formsControl.CornerRadius;
			nativeControl.UpdateBorder(formsControl.BorderColor, formsControl.BorderThickness);
		}

		public static void UpdateFrom(this UIView nativeControl, RoundedBoxView formsControl,
		  string propertyChanged)
		{
			if (nativeControl == null || formsControl == null)
				return;

			if (propertyChanged == RoundedBoxView.CornerRadiusProperty.PropertyName)
			{
				nativeControl.Layer.CornerRadius = (float)formsControl.CornerRadius;
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

		public static void UpdateBorder(this UIView nativeControl, Color color, int thickness)
		{
			nativeControl.Layer.BorderColor = color.ToCGColor();
			nativeControl.Layer.BorderWidth = thickness;
		}
	}
}
