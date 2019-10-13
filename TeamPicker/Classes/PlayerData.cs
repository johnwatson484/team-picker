using System;
using System.Collections.Generic;

namespace TeamPicker.Classes
{
    [Serializable]
    public class PlayerData
    {
        public decimal Version { get; set; }
        public List<Player> Players { get; set; }
    }
}