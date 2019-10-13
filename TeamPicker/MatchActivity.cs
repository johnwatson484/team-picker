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
using TeamPicker.Helpers;
using TeamPicker.Logic;
using TeamPicker.Adapters;
using Plugin.Share;
using Plugin.Share.Abstractions;
using Android.Gms.Ads;

namespace TeamPicker
{
    [Activity(Theme = "@style/Theme.MyTheme")]
    public class MatchActivity : Activity
    {
        MatchLogic mLogic = new MatchLogic();
        Match match;
        PlayerData selectedPlayers;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Match);

            AdView adView = FindViewById<AdView>(Resource.Id.adView);
            AdRequest adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);

            Button saveTeams = FindViewById<Button>(Resource.Id.btnMatchSave);
            Button rePickTeams = FindViewById<Button>(Resource.Id.btnMatchRePick);
            ImageButton cancelMatch = FindViewById<ImageButton>(Resource.Id.btnCancelMatch);
            ImageButton deleteMatch = FindViewById<ImageButton>(Resource.Id.btnDeleteMatch);
            ImageButton shareMatch = FindViewById<ImageButton>(Resource.Id.btnShareMatch);

            string mode = Intent.GetStringExtra("Mode");

            if (mode == "Create")
            {
                string playerDataString = Intent.GetStringExtra("selectedPlayerData");

                selectedPlayers = JsonHelper.FromJSON<PlayerData>(playerDataString);

                match = mLogic.PickTeams(selectedPlayers);

                deleteMatch.Visibility = ViewStates.Gone;                                                
            }
            else
            {
                Guid matchID = Guid.Parse(mode);

                match = mLogic.SelectByID(matchID);
                
                rePickTeams.Visibility = ViewStates.Gone;
            }

            ListView teamListView = FindViewById<ListView>(Resource.Id.listTeams);

            teamListView.Adapter = new TeamListAdapter(this, match.Teams, mode);
            
            saveTeams.Click += (sender, args) =>
            {
                if (mode == "Create")
                {
                    mLogic.Create(match);                    
                }
                else
                {
                    mLogic.Update(match);
                }

                StartActivity(typeof(MatchListActivity));

                Finish();
            };

            rePickTeams.Click += (sender, args) =>
            {
                match = mLogic.PickTeams(selectedPlayers, true, true);

                teamListView.Adapter = new TeamListAdapter(this, match.Teams, "Create");

                Toast.MakeText(this, "Teams Regenerated.", ToastLength.Short).Show();
            };

            cancelMatch.Click += (sender, args) =>
            {
                if (mode != "Create")
                {
                    StartActivity(typeof(MatchListActivity));
                }

                Finish();                
            };

            deleteMatch.Click += (sender, args) =>
            {
                int totalTeams = match.Teams.Count;

                AlertDialog.Builder builder = new AlertDialog.Builder(this);

                AlertDialog alertDialog = builder.Create();

                alertDialog.SetTitle("Delete Match?");

                alertDialog.SetIcon(Android.Resource.Drawable.IcMenuDelete);

                string message = string.Format("Are you sure you wish to delete this match from {0} for {1} teams?", match.DatePicked.ToShortDateString(), totalTeams.ToString());

                alertDialog.SetMessage(message);

                alertDialog.SetButton("OK", (s, ev) =>
                {
                    mLogic.Delete(match.MatchID);

                    Toast.MakeText(this, "Match deleted.", ToastLength.Short).Show();

                    Intent intent = new Intent(this, typeof(SelectionActivity));

                    intent.SetFlags(ActivityFlags.ClearTop);

                    StartActivity(intent);

                    Intent intent2 = new Intent(this, typeof(MatchListActivity));                    

                    StartActivity(intent2);

                    Finish();
                });

                alertDialog.SetButton2("Cancel", (s, ev) =>
                {

                });

                alertDialog.Show();
            };

            shareMatch.Click += (sender, args) =>
            {
                if(CrossShare.IsSupported)
                {
                    CrossShare.Current.Share(new ShareMessage
                    {
                        Title = "I just picked these teams using Team Picker!",
                        Text = StringHelper.WriteMatch(match)
                    },
                    new ShareOptions
                    {
                        ChooserTitle = "Share Teams"
                    });
                }
            };
        }
    }
}