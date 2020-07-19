using System;
using System.Text;
using TeamPicker.Classes;

namespace TeamPicker.Helpers
{
    public static class StringHelper
    {
        public static string WriteMatch(Match match)
        {
            var sb = new StringBuilder();
            sb.AppendLine();

            foreach (var team in match.Teams)
            {
                sb.AppendLine(team.TeamName);

                foreach (var player in team.Players)
                {
                    sb.AppendLine(player.PlayerName);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}