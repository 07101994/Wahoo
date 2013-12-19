using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wahoo.iOS.Data
{

    /// <summary>
    /// A class containing the data item, in this example flowers.
    /// </summary>
    public class Flower
    {

        #region properties

        public string ImagePath { get; set; } // Needed to set the background.
        public string Title { get; set; } // Sets the title at the top.
        public string Subtitle { get; set; } // Sets the subtitle at the top.

        #endregion

    }

}
