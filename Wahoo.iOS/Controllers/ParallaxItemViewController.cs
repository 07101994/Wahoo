using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Wahoo.iOS.Blocks;
using Wahoo.iOS.Data;
using Wahoo.iOS.Views;

namespace Wahoo.iOS.Controllers
{

    /// <summary>
    /// The view controller containing a single item.
    /// </summary>
    [Register("ParallaxItemViewController")]
    public class ParallaxItemViewController : UIViewController
    {

        #region members

        private ParallaxBlurView scroll; // The scroller.
        private Flower flowerItem; // The datasource for the scroller.

        #endregion

        #region constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="flower"></param>
        public ParallaxItemViewController(Flower flower)
        {
            flowerItem = flower;
        }

        #endregion

        #region overrides

        /// <summary>
        /// Handles when a memory warning has occurred.
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        /// <summary>
        /// Handles when the view has loaded.
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                // Perform any additional setup after loading the view
                base.ViewDidLoad();

                // Create a new parallaxview.
                scroll = new ParallaxBlurView(this.View.Frame);
                scroll.BackgroundImage = UIImage.FromBundle(flowerItem.ImagePath);
                scroll.HeaderTitle = flowerItem.Title;
                scroll.HeaderSubtitle = flowerItem.Subtitle;
                scroll.ScrollViewContentSize = new SizeF(320, 1020); //TODO: Remove, is debug only.
                
                this.View.Add(scroll);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion

        #region methods

        /// <summary>
        /// Handles when the header is tapped, we scroll back up.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeaderTapped(object sender, EventArgs e)
        {
            scroll.ScrollTo(0, 0);
        }

        #endregion

    }

}
