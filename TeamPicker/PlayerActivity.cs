using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using TeamPicker.Classes;
using TeamPicker.Logic;

namespace TeamPicker
{
    [Activity(Theme = "@style/Theme.MyTheme")]
    public class PlayerActivity : Activity
    {
        readonly PlayerLogic pLogic = new PlayerLogic();
        readonly SettingsLogic sLogic = new SettingsLogic();
        Player player;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Player);

            AdView adView = FindViewById<AdView>(Resource.Id.adView);
            AdRequest adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);

            var maxRating = sLogic.Select().MaximumRating;
            FindViewById<TextView>(Resource.Id.txtRatingLimit).Text = string.Format("(1-{0})", maxRating);

            string mode = Intent.GetStringExtra("Mode");

            if (mode == "Create")
            {
                player = new Player();
            }
            else
            {
                Guid playerID = Guid.Parse(mode);

                player = pLogic.SelectByID(playerID);

                FindViewById<TextView>(Resource.Id.txtPlayerNameInput).Text = player.PlayerName;
                FindViewById<TextView>(Resource.Id.txtNotes).Text = player.Notes;
                FindViewById<TextView>(Resource.Id.numRatingInput).Text = player.Rating.ToString();
                if (player.UniqueRole)
                {
                    FindViewById<CheckBox>(Resource.Id.chkUniqueRole).Checked = true;
                }
            }

            Button savePlayer = FindViewById<Button>(Resource.Id.btnSavePlayer);
            ImageButton deletePlayer = FindViewById<ImageButton>(Resource.Id.btnDeletePlayer);
            ImageButton cancelPlayer = FindViewById<ImageButton>(Resource.Id.btnCancelPlayer);

            if (mode == "Create")
            {
                deletePlayer.Visibility = ViewStates.Invisible;
            }

            savePlayer.Click += (sender, args) =>
            {
                string playerName = FindViewById<TextView>(Resource.Id.txtPlayerNameInput).Text;
                string rating = FindViewById<TextView>(Resource.Id.numRatingInput).Text;
                bool unique = FindViewById<CheckBox>(Resource.Id.chkUniqueRole).Checked;
                string notes = FindViewById<TextView>(Resource.Id.txtNotes).Text;

                if (string.IsNullOrEmpty(playerName))
                {
                    Toast.MakeText(this, "Please supply Player Name.", ToastLength.Short).Show();
                }
                else if (string.IsNullOrEmpty(rating))
                {
                    Toast.MakeText(this, "Please supply Rating.", ToastLength.Short).Show();
                }
                else if (Convert.ToInt32(rating) > maxRating || Convert.ToInt32(rating) < 1)
                {
                    Toast.MakeText(this, string.Format("Rating must be between 1 and {0}.", maxRating), ToastLength.Short).Show();
                }
                else
                {
                    player.PlayerName = playerName;
                    player.Rating = Convert.ToInt32(rating);
                    player.UniqueRole = unique;
                    player.Notes = notes;

                    if (mode == "Create")
                    {
                        player.Selections = 0;
                        pLogic.Create(player);
                    }
                    else
                    {
                        pLogic.Update(player);
                    }

                    Intent intent = new Intent(this, typeof(SelectionActivity));
                    intent.SetFlags(ActivityFlags.ClearTop);

                    StartActivity(intent);

                    Finish();
                }
            };

            cancelPlayer.Click += (sender, args) =>
            {
                Finish();
            };

            deletePlayer.Click += (sender, args) =>
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);

                AlertDialog alertDialog = builder.Create();

                alertDialog.SetTitle("Delete Player?");

                alertDialog.SetIcon(Android.Resource.Drawable.IcMenuDelete);

                string message = string.Format("Are you sure you wish to delete {0}?", player.PlayerName);

                alertDialog.SetMessage(message);

                alertDialog.SetButton("OK", (s, ev) =>
                {
                    pLogic.Delete(player.PlayerID);

                    string tMessage = string.Format("{0} deleted.", player.PlayerName);

                    Toast.MakeText(this, tMessage, ToastLength.Short).Show();

                    Intent intent = new Intent(this, typeof(SelectionActivity));

                    intent.SetFlags(ActivityFlags.ClearTop);

                    StartActivity(intent);

                    Finish();
                });

                alertDialog.SetButton2("Cancel", (s, ev) =>
                {

                });

                alertDialog.Show();
            };
        }
    }
}