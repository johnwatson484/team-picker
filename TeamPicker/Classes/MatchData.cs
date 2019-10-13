using System;
using System.Collections.Generic;

namespace TeamPicker.Classes
{
    [Serializable]
    public class MatchData
    {
        public decimal Version { get; set; }
        public List<Match> Matches { get; set; }
    }
}