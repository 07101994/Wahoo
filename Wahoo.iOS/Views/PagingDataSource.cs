using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;

namespace Wahoo.iOS.Views
{

    /// <summary>
    /// Handles paging between view controllers.
    /// </summary>
    public class PagingDataSource : UIPageViewControllerDataSource
    {

        private LinkedListNode<UIViewController> pages; // List of UIViewControllers to page through.

        /// <summary>
        /// Initialize the list.
        /// </summary>
        /// <param name="controllers"></param>
        public PagingDataSource(IEnumerable<UIViewController> controllers)
        {
            var controllerList = new LinkedList<UIViewController>(controllers);
            pages = controllerList.First;
        }

        /// <summary>
        /// Gets the previous view controller.
        /// </summary>
        /// <param name="pageViewController"></param>
        /// <param name="referenceViewController"></param>
        /// <returns></returns>
        public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var __c = pages.List.Find(referenceViewController);

            if (__c.Previous != null)
                return __c.Previous.Value;
            else
                return null;
        }

        /// <summary>
        /// Gets the next view controller.
        /// </summary>
        /// <param name="pageViewController"></param>
        /// <param name="referenceViewController"></param>
        /// <returns></returns>
        public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var __c = pages.List.Find(referenceViewController);

            if (__c.Next != null)
                return __c.Next.Value;
            else
                return null;
        }

    }

}
