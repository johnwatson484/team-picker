using System;

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