using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Wahoo.iOS.Blocks
{

    [Register("DummyBlock")]
    public class DummyBlock : UIView
    {

        #region members

        private UILabel titleLabel;
        private UIView block;

        #endregion

        #region properties

        /// <summary>
        /// Represents the title to display.
        /// </summary>
        public string Title
        {
            get 
            {
                if (titleLabel != null)
                    return titleLabel.Text;
                else
                    return string.Empty;
            }
            set
            {
                if (titleLabel != null)
                    titleLabel.Text = value;
            }
        }

        #endregion

        #region constructors

        /// <summary>
        /// Constructor for a DummyBlock.
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="title"></param>
        public DummyBlock(string title)
            : base()
        {
            Initialize(title);
        }

        #endregion

        #region initialization

        /// <summary>
        /// Initialize any subviews.
        /// </summary>
        /// <param name="title"></param>
        void Initialize(string title)
        {
            titleLabel = new UILabel()
            {
                Frame = new RectangleF(0, 0, this.Frame.Width, this.Frame.Height), 
                Text = title,
                Font = UIFont.FromName("HelveticaNeue-UltraLight", 39.0f),
                TextColor = UIColor.White,
                ShadowOffset = new SizeF(0, 1),
                ShadowColor = UIColor.Black,
                AdjustsFontSizeToFitWidth = true
            };

            block = new UIView()
            {
                BackgroundColor = Theme.BlockBackgroundColor,
                Frame = new RectangleF(0, titleLabel.Frame.Height + 5, this.Frame.Width, 100)
            };

            // Add our controls.
            this.Add(titleLabel);
            this.Add(block);
        }

        #endregion

    }

}
