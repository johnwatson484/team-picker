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
using TeamPicker.Classes;

namespace TeamPicker.Helpers
{
    public static class StringHelper
    {
        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();

            Array.Reverse(arr);

            return new string(arr);
        }

        public static string WriteMatch(Match match)
        {
            var sb = new StringBuilder();
            sb.AppendLine();

            foreach(var team in match.Teams)
            {
                sb.AppendLine(team.TeamName);

                foreach(var player in team.Players)
                {
                    sb.AppendLine(player.PlayerName);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}