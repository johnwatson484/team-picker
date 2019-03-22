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
using Android.Text;

namespace TeamPicker.Adapters
{
    public class TeamListAdapter : BaseAdapter<Team>
    {
        List<Team> items;
        Activity context;
        string mode;

        public TeamListAdapter(Activity context, List<Team> items, string mode) : base()
        {
            this.context = context;
            this.items = items;
            this.mode = mode;
        }

        public override Team this[int position]
        {
            get
            {
                return items[position];
            }
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;

            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.TeamItem, null);
            }

            view.FindViewById<TextView>(Resource.Id.txtTeamName).Text = item.TeamName;
            view.FindViewById<TextView>(Resource.Id.txtTeamRating).Text = string.Format("({0})", item.Players.Sum(x=>x.Rating).ToString());
            

            if (mode == "Create")
            {
                view.FindViewById<TextView>(Resource.Id.txtTeamScore).Visibility = ViewStates.Invisible;
                view.FindViewById<TextView>(Resource.Id.teamScore).Visibility = ViewStates.Invisible;
            }
            
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < item.Players.Count; i++)
            {
                Player player = item.Players[i];

                sb.AppendLine(player.PlayerName);
            }

            view.FindViewById<TextView>(Resource.Id.txtTeamPlayers).Text = sb.ToString();
            
            TextView score = view.FindViewById<TextView>(Resource.Id.teamScore);

            score.TextChanged -= UpdateScore;

            view.FindViewById<TextView>(Resource.Id.teamScore).Text = item.Score.ToString();

            view.Tag = item.TeamID.ToString();
            score.Tag = position;
            
            score.TextChanged += UpdateScore;

            return view;
        }        

        private void UpdateScore(object sender, TextChangedEventArgs e)
        {
            TextView score = (TextView)sender;         

            int position = Convert.ToInt32(score.Tag.ToString());
                        
            var item = items[position];

            var x = e.Text;
            var y = e.Text.ToString();

            int newScore = 0;

            if(!string.IsNullOrEmpty(e.Text.ToString()))
            {
                newScore = Convert.ToInt32(score.Text.ToString());
            }

            item.Score = Convert.ToInt32(newScore);
        }
    }
}