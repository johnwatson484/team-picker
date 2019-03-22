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
    public class Team
    {
        public Guid TeamID { get; set; }

        public int TeamNumber { get; set; }

        public string TeamName { get; set; }

        public List<Player> Players { get; set; }

        public int Rating
        {
            get
            {
                return Players.Sum(x => x.Rating);
            }
        }

        public int Score { get; set; }

        public int MaxPlayers { get; set; }

        public Team()
        {
            TeamID = Guid.NewGuid();
            Players = new List<Player>();
            MaxPlayers = 0;
        }

        public Team(int number):this()
        {
            TeamName = string.Format("Team {0}", number);
            TeamNumber = number;
        }
    }
}