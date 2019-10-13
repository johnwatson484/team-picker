namespace TeamPicker.Classes
{
    public class PlayerSelection
    {
        public Player Player { get; set; }

        public bool Selected { get; set; }

        public bool IsSelected()
        {
            if (Selected)
            {
                return true;
            }
            return false;
        }
    }
}