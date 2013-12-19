using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Wahoo.iOS.Views
{

    /// <summary>
    /// Represents a view that contains a background image with scrollable content.
    /// Creates a parallax effect when scrolling, while blurring the background picture in the process.
    /// Fits in perfectly because it is combined with a ParallaxBlurHeaderView.
    /// </summary>
    [Register("ParallaxBlurView")]
    public class ParallaxBlurView : UIView, IDisposable
    {

        #region members

        #region variables

        private float parallaxSpeedRatio = 20f; // The speed at which the parallax image moves up and down.
        private float snappingRatio = 3; // Below one third, it snaps downwards. Otherwise upwards.
        private float darkFadeMaxOpacity = 0.3f; // The maximum opacity for the dark background overlay on scrolling.
        private float maxBlurValue = 350f; // At which offset the blurred image is fully visible.
        private float defaultScrollOffset = 0; // The offset at which items for the scrollview will be placed.
        private string headerTitle = "Main title"; // The main title for the header.
        private string headerSubtitle = "Subtitle"; // The subtitle for the header.

        #endregion

        #region controls

        // The controls on this view.
        private UIScrollView parallaxScrollView;
        private UIImageView backgroundPhoto;
        private UIImageView backgroundPhotoWithImageEffects;
        private UIView darkerLayer;
        private FadingScrollView scrollView;
        private ParallaxBlurHeaderView header;

        #endregion

        #endregion

        #region properties

        /// <summary>
        /// The speed at which the parallax image moves up and down.
        /// Default value is 20.0f.
        /// </summary>
        public float ParallaxSpeedRatio
        {
            get { return parallaxSpeedRatio; }
            set { parallaxSpeedRatio = value; }
        }
        
        /// <summary>
        /// The snapping ratio used. This value represents the percentual point at which the snap will occur in a specific direction.
        /// Default value is 3, which means that at 1/3rd of the screen (from the bottom) it will snap downwards, otherwise it will snap upwards.
        /// </summary>
        public float SnappingRatio
        {
            get { return snappingRatio; }
            set { snappingRatio = value; }
        }
        
        /// <summary>
        ///  The maximum opacity for the dark background overlay on scrolling. 
        ///  Values have to be between 0 and 1.0, default is 0.3.
        /// </summary>
        public float DarkFadeMaxOpacity
        {
            get { return darkFadeMaxOpacity; }
            set { darkFadeMaxOpacity = value; }
        }
        
        /// <summary>
        /// Defines at which offset value the blurred image is fully visible.
        /// </summary>
        public float MaxBlurValue
        {
            get { return maxBlurValue; }
            set { maxBlurValue = value; }
        }

        /// <summary>
        /// The offset at which items for the scrollview will be placed.
        /// </summary>
        public float DefaultScrollOffset
        {
            get { return defaultScrollOffset; }
            set { defaultScrollOffset = value; }
        }

        /// <summary>
        /// Sets the content size for the scrollview.
        /// </summary>
        public SizeF ScrollViewContentSize
        {
            get { return scrollView.ContentSize; }
            set { scrollView.ContentSize = value; }
        }

        /// <summary>
        /// The background image for the view.
        /// </summary>
        public UIImage BackgroundImage
        {
            get { return backgroundPhoto.Image; }
            set { backgroundPhoto.Image = value; backgroundPhotoWithImageEffects.Image = value.Blur(); }
        }

        /// <summary>
        /// The title for the header of the view.
        /// </summary>
        public string HeaderTitle
        {
            get { return headerTitle; }
            set { headerTitle = value; header.HeaderTitle = headerTitle; }
        }

        /// <summary>
        /// The subtitle for the header of the view.
        /// </summary>
        public string HeaderSubtitle
        {
            get { return headerSubtitle; }
            set { headerSubtitle = value; header.HeaderSubtitle = headerSubtitle; }
        }

        #endregion

        #region constructors

        /// <summary>
        /// The default parameterless constructor.
        /// </summary>
        public ParallaxBlurView()
            : base()
        {
            Initialize();
        }

        /// <summary>
        /// The default constructor taking a RectangleF as the Frame.
        /// </summary>
        public ParallaxBlurView(RectangleF bounds)
            : base(bounds)
        {
            Initialize();
        }

        #endregion

        #region init methods

        /// <summary>
        /// Initializes all the parts and controls of the parallaxing screen.
        /// </summary>
        /// <param name="background"></param>
        void Initialize()
        {
            // Set up the parallax.
            parallaxScrollView = new UIScrollView(this.Frame);

            // Set the default scroll height for the frame. 
            // This is the offset where the content goes when it's snapped to the top.
            defaultScrollOffset = this.Frame.Height - 130 - 60 - 40;
            
            // Set up our darkening layer.
            darkerLayer = new UIView(this.Frame);
            darkerLayer.BackgroundColor = UIColor.Black;
            darkerLayer.Alpha = 0;

            // Set up our scrolling.
            scrollView = new Views.FadingScrollView(this.Frame)
            {
                ShowsHorizontalScrollIndicator = false,
                ShowsVerticalScrollIndicator = false,
                DecelerationRate = UIScrollView.DecelerationRateNormal
            };
            
            // Add some events to handle the scrolling.
            scrollView.Scrolled += ScrollViewDidScroll;
            scrollView.DraggingEnded += ScrollViewDraggingEnded;
            scrollView.DecelerationEnded += ScrollViewDecelerationEnded;

            // Makes sure that a tap scrolls the view up a bit.
            var tap = new UITapGestureRecognizer();
            tap.AddTarget(() => { if (scrollView.ContentOffset.Y == 0) ScrollTo(0, defaultScrollOffset); });
            scrollView.AddGestureRecognizer(tap);

            // Setup the picture frame. We make it higher then the view is due to parallax.
            RectangleF frame = this.Frame;
            frame.Size = new SizeF(frame.Width, frame.Height + frame.Height / parallaxSpeedRatio);

            // Set up the pictures.
            backgroundPhoto = new UIImageView(frame) { 
                AutoresizingMask = UIViewAutoresizing.FlexibleHeight,
                ContentMode = UIViewContentMode.ScaleAspectFill           
            };

            backgroundPhotoWithImageEffects = new UIImageView(frame) { 
                ContentMode = UIViewContentMode.ScaleAspectFill,
                AutoresizingMask = UIViewAutoresizing.FlexibleHeight,
                Alpha = 0
            };
                        
            // Create the header.
            header = new ParallaxBlurHeaderView(new RectangleF(0, 0, 320, 50));
            header.OnTapped += HeaderOnTapped;

            // Add our items to the view.
            parallaxScrollView.Add(backgroundPhoto);
            parallaxScrollView.Add(backgroundPhotoWithImageEffects);

            // Do the actual cross dissolve effect.
            CrossDissolvePhotos(backgroundPhoto.Image, backgroundPhotoWithImageEffects.Image);

            // Add our elements to the view.
            this.Add(parallaxScrollView);
            this.Add(darkerLayer);
            this.Add(scrollView);
            this.Add(header);
        }

        /// <summary>
        /// Lays out the subviews of this view for parallax usage.
        /// </summary>
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            // Make our images bigger then the view, so we can do a parallax.
            // The additional offset is needed for when the view overflows.
            RectangleF frame = this.Frame;
            frame.Size = new SizeF(frame.Width, frame.Height + frame.Height / parallaxSpeedRatio);

            // Frame the pictures correctly so we can do a parallax effect.
            parallaxScrollView.Frame = this.Frame;
            backgroundPhoto.Frame = frame;
            backgroundPhotoWithImageEffects.Frame = frame;
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

        #region methods

        /// <summary>
        /// Handles when the header is being tapped.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeaderOnTapped(object sender, EventArgs e)
        {
            ScrollTo(0, 0);
        }

        /// <summary>
        /// Add a view to the scroller.
        /// </summary>
        /// <param name="view"></param>
        public void AddItemToScrollView(UIView view)
        {
            if (scrollView != null)
                scrollView.Add(view);
        }

        /// <summary>
        /// Performs the view transition with a cross dissolve of the pictures.
        /// </summary>
        /// <param name="photo"></param>
        private void CrossDissolvePhotos(UIImage photo, UIImage photoWithEffects)
        {
            UIView.Transition(this.backgroundPhoto, 1.0f, UIViewAnimationOptions.TransitionCrossDissolve, delegate
            {
                this.backgroundPhoto.Image = photo;
                this.backgroundPhotoWithImageEffects.Image = photoWithEffects;
            }, null);
        }

        #endregion

        #region scrolling

        /// <summary>
        /// Scroll the scrollview to a given position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void ScrollTo(float x, float y)
        {
            if (scrollView != null)
            {
                // Slows the scrolling down.
                //UIView.BeginAnimations("scrollAnimation");
                //UIView.SetAnimationDuration(0.8);
                scrollView.SetContentOffset(new PointF(x, y), true);
                //UIView.CommitAnimations();
            }
        }

        /// <summary>
        /// Scrolls the view to it's default snapping position.
        /// </summary>
        private void ScrollToSnapPosition()
        {
            if (scrollView.ContentOffset.Y > 0 && scrollView.ContentOffset.Y <= scrollView.Frame.Height / snappingRatio)
            {
                // When in the bottom part of the SnappingRatio, we snap to the original position.
                ScrollTo(0, 0);
            }
            else if (scrollView.ContentOffset.Y > scrollView.Frame.Height / snappingRatio && scrollView.ContentOffset.Y <= scrollView.Frame.Height)
            {
                // When in the top part of the SnappingRatio, we snap to the top.
                ScrollTo(0, DefaultScrollOffset);
            }
        }

        /// <summary>
        /// Handles when the deceleration of the scrollview ended so we can snap into place.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewDecelerationEnded(object sender, EventArgs e)
        {
            ScrollToSnapPosition();
        }

        /// <summary>
        /// Handles when the dragging of the scrollview ended so we can snap into place.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewDraggingEnded(object sender, DraggingEventArgs e)
        {
            if (e.Decelerate == false)
            {
                ScrollToSnapPosition();
            }
        }

        /// <summary>
        /// Handles the scrolling. When we're past blurValue, the item is fully blurred.
        /// Play around with the blurValue to achieve the effect you are looking for.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollViewDidScroll(object sender, EventArgs e)
        {
            // Set our parallax offset.
            if (scrollView.ContentOffset.Y / parallaxSpeedRatio > 0.0f && 
                scrollView.ContentOffset.Y < parallaxScrollView.Frame.Size.Height &&
                scrollView.ContentOffset.Y <= defaultScrollOffset)
            {
                parallaxScrollView.SetContentOffset(new PointF(0.0f, scrollView.ContentOffset.Y / parallaxSpeedRatio), false);
            }

            // Check if we need to fade into blurred image.
            if (scrollView.ContentOffset.Y > 0 && scrollView.ContentOffset.Y <= maxBlurValue)
            {
                // Percentually fade in the blurred image and fade in the dark overlay.
                float percent = (float)(scrollView.ContentOffset.Y / maxBlurValue);
                backgroundPhotoWithImageEffects.Alpha = percent;
                darkerLayer.Alpha = darkFadeMaxOpacity * (scrollView.ContentOffset.Y / maxBlurValue);
            }
            else if (scrollView.ContentOffset.Y > maxBlurValue)
            {
                // Our blurred image and dark overlay are now fully visible.
                backgroundPhotoWithImageEffects.Alpha = 1;
                darkerLayer.Alpha = darkFadeMaxOpacity;
            }
            else if (scrollView.ContentOffset.Y <= 0)
            {
                // Our blurred image and dark overlay are now fully invisible.
                backgroundPhotoWithImageEffects.Alpha = 0;
                darkerLayer.Alpha = 0;
            }
        }

        #endregion

    }

}