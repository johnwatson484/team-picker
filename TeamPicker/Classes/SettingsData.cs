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
    public class SettingsData
    {
        public decimal Version { get; set; }

        public int NumberOfTeams { get; set; }

        public int MaximumRating { get; set; }

        public bool BalanceTeams { get; set; }

        public string OrderBy { get; set; }

        public bool TrafficLightRatings { get; set; }

        public bool HighAccuracy { get; set; }

        public bool DisplayNotes { get; set; }
    }
}