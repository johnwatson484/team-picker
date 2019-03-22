using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TeamPicker.Classes
{
    public class PlayerSelection
    {
        public Player Player { get; set; }

        public bool Selected { get; set; }

        public bool IsSelected()
        {
            if (Selected)
            {
                return true;
            }
            return false;
        }
    }
}