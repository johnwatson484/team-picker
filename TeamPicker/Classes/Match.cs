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
    public class Match
    {
        public Guid MatchID { get; set; }

        public DateTime DatePicked { get; set; }

        public List<Team> Teams { get; set; }

        public Match()
        {
            Teams = new List<Team>();
        }
    }
}