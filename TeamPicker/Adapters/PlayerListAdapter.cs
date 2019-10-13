using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using TeamPicker.Classes;

namespace TeamPicker.Adapters
{
    public class PlayerListAdapter : BaseAdapter<PlayerSelection>
    {
        readonly List<PlayerSelection> items;
        readonly Activity context;
        readonly bool trafficLightRatings;
        readonly bool displayNotes;
        readonly int maxRating;

        public PlayerListAdapter(Activity context, List<PlayerSelection> items, bool trafficLightRatings, bool displayNotes, int maxRating) : base()
        {
            this.context = context;
            this.items = items;
            this.trafficLightRatings = trafficLightRatings;
            this.displayNotes = displayNotes;
            this.maxRating = maxRating;
        }

        public override PlayerSelection this[int position]
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
                view = context.LayoutInflater.Inflate(Resource.Layout.PlayerListItem, null);
            }

            view.FindViewById<TextView>(Resource.Id.playerName).Text = item.Player.PlayerName;
            view.FindViewById<TextView>(Resource.Id.rating).Text = item.Player.Rating.ToString();

            if (displayNotes)
            {
                view.FindViewById<TextView>(Resource.Id.notes).Visibility = ViewStates.Visible;
                view.FindViewById<TextView>(Resource.Id.notes).Text = item.Player.Notes;
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.notes).Visibility = ViewStates.Gone;
            }

            if (item.Player.UniqueRole)
            {
                view.FindViewById<TextView>(Resource.Id.uniqueRole).Visibility = ViewStates.Visible;
                view.FindViewById<TextView>(Resource.Id.uniqueRole).Text = "Unique Role";
            }
            else
            {
                view.FindViewById<TextView>(Resource.Id.uniqueRole).Visibility = ViewStates.Gone;
            }

            if (trafficLightRatings)
            {
                int set1 = (int)Math.Ceiling(((double)maxRating / 100) * 17);
                int set2 = set1 * 2;
                int set3 = set1 * 3;
                int set4 = set1 * 4;

                if (item.Player.Rating >= 1 && item.Player.Rating <= set1)
                {
                    view.FindViewById<TextView>(Resource.Id.rating).SetTextColor(Color.ParseColor("#cc3232"));
                }
                else if (item.Player.Rating > set1 && item.Player.Rating <= set2)
                {
                    view.FindViewById<TextView>(Resource.Id.rating).SetTextColor(Color.ParseColor("#db7b2b"));
                }
                else if (item.Player.Rating > set2 && item.Player.Rating <= set3)
                {
                    view.FindViewById<TextView>(Resource.Id.rating).SetTextColor(Color.ParseColor("#e7b416"));
                }
                else if (item.Player.Rating > set3 && item.Player.Rating <= set4)
                {
                    view.FindViewById<TextView>(Resource.Id.rating).SetTextColor(Color.ParseColor("#99c140"));
                }
                else if (item.Player.Rating > set4 && item.Player.Rating <= maxRating)
                {
                    view.FindViewById<TextView>(Resource.Id.rating).SetTextColor(Color.ParseColor("#2dc937"));
                }
                else
                {
                    view.FindViewById<TextView>(Resource.Id.rating).SetTextColor(Color.ParseColor("#000000"));
                }
            }

            CheckBox chkBox = view.FindViewById<CheckBox>(Resource.Id.chkPlayer);

            view.LongClick -= OpenUpdatePlayer;
            chkBox.CheckedChange -= UpdateCheckStatus;

            view.Tag = item.Player.PlayerID.ToString();
            chkBox.Tag = position;

            chkBox.Checked = item.IsSelected();

            view.LongClick += OpenUpdatePlayer;
            chkBox.CheckedChange += UpdateCheckStatus;

            return view;
        }

        private void OpenUpdatePlayer(object sender, EventArgs e)
        {
            View view = (View)sender;

            string playerIDString = view.Tag.ToString();

            Intent intent = new Intent(context, typeof(PlayerActivity));

            intent.PutExtra("Mode", playerIDString);

            context.StartActivity(intent);
        }

        private void UpdateCheckStatus(object sender, EventArgs e)
        {
            CheckBox chkBox = (CheckBox)sender;

            int position = Convert.ToInt32(chkBox.Tag.ToString());

            var item = items[position];

            int selected = Convert.ToInt32(context.FindViewById<TextView>(Resource.Id.selectedNumber).Text);

            if (chkBox.Checked)
            {
                item.Selected = true;
                selected++;
            }
            else
            {
                item.Selected = false;
                selected--;
            }

            context.FindViewById<TextView>(Resource.Id.selectedNumber).Text = selected.ToString();
        }
    }
}