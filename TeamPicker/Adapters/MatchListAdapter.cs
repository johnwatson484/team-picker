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

namespace TeamPicker.Adapters
{
    public class MatchListAdapter : BaseAdapter<Match>
    {
        List<Match> items;
        Activity context;

        public MatchListAdapter(Activity context, List<Match> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override Match this[int position]
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
                view = context.LayoutInflater.Inflate(Resource.Layout.MatchListItem, null);
            }

            int totalTeams = item.Teams.Count;

            string totalTeamsString = string.Format("{0} Teams", totalTeams.ToString());

            view.FindViewById<TextView>(Resource.Id.txtTotalPlayers).Text = totalTeamsString;
            view.FindViewById<TextView>(Resource.Id.txtMatchDate).Text = item.DatePicked.ToShortDateString();
            view.FindViewById<TextView>(Resource.Id.txtMatchTime).Text = item.DatePicked.ToShortTimeString();

            view.FindViewById<TextView>(Resource.Id.txtScore).Text = null;
            
            view.Click -= OpenViewMatch;            
            
            view.Tag = item.MatchID.ToString();            

            view.Click += OpenViewMatch;            

            return view;
        }

        private void OpenViewMatch(object sender, EventArgs e)
        {
            View view = (View)sender;

            string matchIDString = view.Tag.ToString();

            Intent intent = new Intent(context, typeof(MatchActivity));

            intent.PutExtra("Mode", matchIDString);

            context.StartActivity(intent);

            context.Finish();
        }  
    }
}