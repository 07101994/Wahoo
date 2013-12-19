using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using MonoTouch.CoreImage;
using MonoTouch.UIKit;

namespace Wahoo.iOS.Views
{

    /// <summary>
    /// A static class to contain all the extension methods.
    /// </summary>
    public static class Extensions
    {

        #region members

        private const float BLUR_RADIUS = 25.0f;

        #endregion

        #region extensions

        /// <summary>
        /// Creates a blurred version of our image in memory.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static UIImage Blur(this UIImage image)
        {
            if (image != null)
            {
                // Create a new blurred image.
                var beginImage = new CIImage(image);
                var blur = new CIGaussianBlur();
                blur.Image = beginImage;
                blur.Radius = BLUR_RADIUS;

                var outputImage = blur.OutputImage;
                //var context = CIContext.FromOptions(null);
                var context = CIContext.FromOptions(new CIContextOptions() { UseSoftwareRenderer = true }); // CPU
                var cgImage = context.CreateCGImage(outputImage, new RectangleF(new PointF(0, 0), image.Size));
                var newImage = UIImage.FromImage(cgImage);

                // Clear up some resources.
                beginImage = null;
                context = null;
                blur = null;
                outputImage = null;
                cgImage = null;

                return newImage;
            }
            else
            {
                return null;
            }
        }

        #endregion

    }

}
