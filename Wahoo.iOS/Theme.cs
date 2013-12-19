using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;

namespace Wahoo.iOS
{

    /// <summary>
    /// A static class that can be used to create a theme.
    /// </summary>
    public static class Theme
    {

        #region colors

        static Lazy<UIColor> blockBackgroundColor = new Lazy<UIColor>(() => UIColor.FromRGBA(0, 0, 0, 128));

        /// <summary>
        /// General semi-transparent black backgrounds in the app.
        /// </summary>
        public static UIColor BlockBackgroundColor
        {
            get { return blockBackgroundColor.Value; }
        }

        #endregion

        #region images

        static Lazy<UIImage> navigationLeftIcon = new Lazy<UIImage>(() => UIImage.FromBundle("Images/InfoIcon.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal));
        static Lazy<UIImage> navigationRightIcon = new Lazy<UIImage>(() => UIImage.FromBundle("Images/ListIcon.png").ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal));

        /// <summary>
        /// Left navbar icon image.
        /// </summary>
        public static UIImage NavigationLeftIcon
        {
            get { return navigationLeftIcon.Value; }
        }

        /// <summary>
        /// Right navbar icon image.
        /// </summary>
        public static UIImage NavigationRightIcon
        {
            get { return navigationRightIcon.Value; }
        }

        #endregion

        #region fonts

        const string FontName = "HelveticaNeue";
        const string BoldFontName = "HelveticaNeue-Bold";
        const string LightFontName = "HelveticaNeue-Light";
        const string UltraLightFontName = "HelveticaNeue-UltraLight";

        /// <summary>
        /// Returns the default font with a certain size
        /// </summary>
        public static UIFont FontOfSize(float size)
        {
            return UIFont.FromName(FontName, size);
        }

        /// <summary>
        /// Returns the default bold font with a certain size
        /// </summary>
        public static UIFont BoldFontOfSize(float size)
        {
            return UIFont.FromName(BoldFontName, size);
        }

        /// <summary>
        /// Returns the default light font with a certain size
        /// </summary>
        public static UIFont LightFontOfSize(float size)
        {
            return UIFont.FromName(LightFontName, size);
        }

        /// <summary>
        /// Returns the default ultra light font with a certain size
        /// </summary>
        public static UIFont UltraLightFontOfSize(float size)
        {
            return UIFont.FromName(UltraLightFontName, size);
        }

        #endregion

        /// <summary>
        /// Applies this theme to everything.
        /// </summary>
        public static void Apply()
        {
        }

    }

}
