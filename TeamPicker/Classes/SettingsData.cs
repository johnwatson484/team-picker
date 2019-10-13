using System;

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