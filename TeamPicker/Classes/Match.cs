using System;
using System.Collections.Generic;

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