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
    public class MatchData
    {
        public decimal Version { get; set; }
        public List<Match> Matches { get; set; }
    }
}