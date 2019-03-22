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
    [Serializable]
    public class Player
    {
        public Guid PlayerID { get; set; }

        public string PlayerName { get; set; }

        public int Rating { get; set; }

        public bool UniqueRole { get; set; }

        public string Notes { get; set; }

        public int Selections { get; set; }

        public Player()
        {
            PlayerID = Guid.NewGuid();
        }
    }
}