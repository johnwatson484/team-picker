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

    public class PlayerMatch
    {
        public Player Player { get; set; }

        public decimal Random { get; set; }

        public decimal Final { get; set; }

        public int Rank { get; set; }

        public string Binary { get; set; }

        public string BinaryReverse { get; set; }

        public int Order { get; set; }
    }
}