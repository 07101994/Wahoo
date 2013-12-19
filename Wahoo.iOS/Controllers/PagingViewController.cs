using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Wahoo.iOS.Data;
using Wahoo.iOS.Views;

namespace Wahoo.iOS.Controllers
{

    /// <summary>
    /// A paging view controller, which handles horizontal scrolling.
    /// </summary>
    public class PagingViewController : UIViewController
    {

        #region members

        private UIPageViewController pageController;
        private UIScrollView horScrollView;
        private UILabel debugOffset;

        #endregion

        #region overrides

        /// <summary>
        /// Handles memory warnings that might occur.
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        /// <summary>
        /// Handles loading the view. Override to perform any additional setup after loading the view.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Handles the spacing between the pages.
            var values = new NSObject[] { NSNumber.FromFloat(4.0f) };
            var keys = new NSObject[] { new NSString("UIPageViewControllerOptionInterPageSpacingKey") };
            var options = NSDictionary.FromObjectsAndKeys(values, keys);

            var controllers = new List<UIViewController>()
            {
                new ParallaxItemViewController(new Flower() {ImagePath = "Images/Samples/Daffodils.jpg", Title="Daffodils", Subtitle="Narcissus"}),
                new ParallaxItemViewController(new Flower() {ImagePath = "Images/Samples/Orchids.jpg", Title="Orchids", Subtitle="Orchidaceae"}),
                new ParallaxItemViewController(new Flower() {ImagePath = "Images/Samples/Roses.jpg", Title="Roses", Subtitle="Rosa"})
            };

            // Init our pageviewer.
            pageController = new UIPageViewController(UIPageViewControllerTransitionStyle.Scroll, UIPageViewControllerNavigationOrientation.Horizontal, options);
            pageController.SetViewControllers(new UIViewController[] { controllers[0] }, UIPageViewControllerNavigationDirection.Forward, false, null);
            pageController.View.Frame = this.View.Bounds;
            pageController.DataSource = new PagingDataSource(controllers);

            // Hooks up the scrollview so we can parallax it.
            SetupParallax();
            
            // Add the controls to the view.
            this.Add(this.pageController.View);
            
            // Create an offset - this is for debugging purposes only.
            debugOffset = new UILabel() { Frame = new RectangleF(10, 100, 300, 50), Text = "Offset = ", Font = Theme.FontOfSize(14f), TextColor = UIColor.White };
            this.Add(debugOffset);
        }

        #endregion

        #region methods

        /// <summary>
        /// Creates the parallax effect by hooking into the scrollview.
        /// </summary>
        private void SetupParallax()
        {
            foreach (var item in this.pageController.View.Subviews)
            {
                // Check if there is a scrollview.
                if (item.GetType() == typeof(UIScrollView))
                {
                    // We have a scrollview!
                    horScrollView = (UIScrollView)item;
                    horScrollView.Scrolled += horizontalScroller_Scrolled;
                }

                return;
            }
        }

        /// <summary>
        /// Handles the scrolling event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void horizontalScroller_Scrolled(object sender, EventArgs e)
        {
            debugOffset.Text = string.Format("Offset = X:{0}, Y:{1}", horScrollView.ContentOffset.X, horScrollView.ContentOffset.Y);
            //_items[this.PageIndex].View.Transform = CGAffineTransform.MakeTranslation(-horizontalScroller.ContentOffset.X, 0);
            //_items[this.PageIndex+1].View.Transform = CGAffineTransform.MakeTranslation(horizontalScroller.ContentOffset.X, 0);
        }

        #endregion

    }

}
