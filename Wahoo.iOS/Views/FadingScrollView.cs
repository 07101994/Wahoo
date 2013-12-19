using System;
using System.Drawing;
using MonoTouch.CoreAnimation;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Wahoo.iOS.Views
{

    /// <summary>
    /// A scrollview where the top part fades.
    /// </summary>
    public class FadingScrollView : UIScrollView
    {

        #region constructor

        /// <summary>
        /// Creates a scrollview based on the given frame.
        /// </summary>
        /// <param name="frame"></param>
        public FadingScrollView(RectangleF frame) : base(frame) { }

        #endregion

        #region overrides

        /// <summary>
        /// Lays out the subviews for this view.
        /// </summary>
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            // Create the colors for our gradient.
            UIColor transparent = UIColor.FromWhiteAlpha(1.0f, 0);
            UIColor opaque = UIColor.FromWhiteAlpha(1.0f, 1.0f);
            
            // Create a masklayer.
            CALayer maskLayer = new CALayer() { Frame = this.Bounds };
            CAGradientLayer gradientLayer = new CAGradientLayer()
            {
                Frame = new RectangleF(this.Bounds.X, 0, this.Bounds.Size.Width, this.Bounds.Size.Height),
                Colors = new CGColor[] { transparent.CGColor, transparent.CGColor, opaque.CGColor, opaque.CGColor },
                Locations = new NSNumber[] { 0.0f, 0.09f, 0.11f, 1.0f }
            };

            // Add the mask.
            maskLayer.AddSublayer(gradientLayer);
            this.Layer.Mask = maskLayer;
        }

        #endregion

    }

}