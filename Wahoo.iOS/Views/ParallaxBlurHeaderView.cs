using System;
using System.Drawing;

using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Wahoo.iOS.Views
{

    /// <summary>
    /// A header containing the title and two images for the parallax view.
    /// </summary>
    [Register("ParallaxBlurHeaderView")]
    public class ParallaxBlurHeaderView : UIView, IDisposable
    {

        #region members

        // Events to handle tapping the header.
        public delegate void TappedHandler(object sender, EventArgs e);
        public TappedHandler OnTapped;

        // Members for the title.
        private string headerTitle = "Title";
        private string headerSubtitle = "Subtitle";
        private UILabel titleLabel, subtitleLabel;
        private float paddingX = 15f;
        private float paddingY = 20f;

        #endregion

        #region properties

        /// <summary>
        /// The title displayed in the header.
        /// </summary>
        public string HeaderTitle
        {
            get { return headerTitle; }
            set { headerTitle = value; titleLabel.Text = value; } 
        }

        /// <summary>
        /// The subtitle displayed in the header.
        /// </summary>
        public string HeaderSubtitle
        {
            get { return headerSubtitle; }
            set { headerSubtitle = value.ToUpper(); subtitleLabel.Text = value.ToUpper(); }
        }
        
        #endregion

        #region constructors

        /// <summary>
        /// Default empty constructor.
        /// </summary>
        public ParallaxBlurHeaderView()
        {
            Initialize();
        }

        /// <summary>
        /// Default constructor with a RectangleF.
        /// </summary>
        /// <param name="bounds"></param>
        public ParallaxBlurHeaderView(RectangleF bounds)
            : base(bounds)
        {
            Initialize();
        }

        #endregion

        #region initialization

        /// <summary>
        /// Initializes the control.
        /// </summary>
        void Initialize()
        {
            // Create the headerbuttons.
            var leftButton = UIButton.FromType(UIButtonType.Custom);
            var rightButton = UIButton.FromType(UIButtonType.Custom);

            // Create the list button.
            leftButton.Frame = new RectangleF(paddingX, paddingY, Theme.NavigationLeftIcon.Size.Width, Theme.NavigationLeftIcon.Size.Height);
            leftButton.SetImage(Theme.NavigationLeftIcon, UIControlState.Normal);
            this.Add(leftButton);

            // Create the info button.
            rightButton.Frame = new RectangleF(this.Bounds.Width - paddingX - Theme.NavigationRightIcon.Size.Width, paddingY, Theme.NavigationRightIcon.Size.Width, Theme.NavigationLeftIcon.Size.Height);
            rightButton.SetImage(Theme.NavigationRightIcon, UIControlState.Normal);
            this.Add(rightButton);

            // Create the title label.
            titleLabel = new UILabel()
            {
                Frame = new RectangleF(46, 10, 228, 22),
                TextColor = UIColor.White,
                Font = Theme.FontOfSize(17f),
                TextAlignment = UITextAlignment.Center,
                ShadowOffset = new SizeF(0, 1),
                ShadowColor = UIColor.Black,
                AdjustsFontSizeToFitWidth = true,
                Text = headerTitle
            };

            // Create the title label.
            subtitleLabel = new UILabel()
            {
                Frame = new RectangleF(46, 30, 228, 22),
                TextColor = UIColor.White,
                Font = Theme.FontOfSize(12f),
                TextAlignment = UITextAlignment.Center,
                ShadowOffset = new SizeF(0, 1),
                ShadowColor = UIColor.Black,
                AdjustsFontSizeToFitWidth = true,
                Text = headerSubtitle
            };

            // Add the labels to the header.
            this.Add(titleLabel);
            this.Add(subtitleLabel);

            // Makes sure that a tap on the header can be handled and bubble it up.
            var tap = new UITapGestureRecognizer();
            tap.AddTarget(() => { Tapped(); });
            this.AddGestureRecognizer(tap);
        }

        /// <summary>
        /// When disposing, we can remove everything from view and memory.
        /// </summary>
        void IDisposable.Dispose()
        {
            // Brute force, remove everything
            foreach (var view in Subviews)
                view.RemoveFromSuperview();
        }

        #endregion

        #region events

        /// <summary>
        /// Bubbles up the tap action, so we can handle a scroll higher in the hierarchy.
        /// </summary>
        private void Tapped()
        {
            // Make sure someone is listening to the event.
            if (OnTapped == null) return;
            OnTapped(this, new EventArgs());
        }

        #endregion

    }
}