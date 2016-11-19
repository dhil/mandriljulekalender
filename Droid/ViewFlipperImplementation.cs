using ViewFlipper.FormsPlugin.Abstractions;
using System;
using Xamarin.Forms;
using ViewFlipper.FormsPlugin.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ViewFlipper.FormsPlugin.Abstractions.ViewFlipper), typeof(ViewFlipperRenderer))]
namespace Mandrilkalender.Droid
{
    /// <summary>
    /// ViewFlipper Renderer
    /// </summary>
    public class ViewFlipperRenderer //: TRender (replace with renderer type
    {
        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public static void Init() { }
    }
}