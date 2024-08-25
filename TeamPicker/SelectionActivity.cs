using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.OS;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.ApplicationModel;
using System.Collections.Generic;
using System.Linq;
using TeamPicker.Adapters;
using TeamPicker.Classes;
using TeamPicker.Helpers;
using TeamPicker.Logic;

namespace TeamPicker
{
    [Activity(Theme = "@style/Theme.MyTheme")]
    public class SelectionActivity : Activity
    {
        readonly PlayerLogic pLogic = new PlayerLogic();
        readonly MatchLogic mLogic = new MatchLogic();
        readonly SettingsLogic sLogic = new SettingsLogic();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.Selection);

            AdView adView = FindViewById<AdView>(Resource.Id.adView);
            AdRequest adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);

            pLogic.CheckFileExists();
            mLogic.CheckFileExists();
            sLogic.CheckFileExists();

            List<PlayerSelection> players = pLogic.SelectAll();

            var currentSettings = sLogic.Select();

            switch (currentSettings.OrderBy)
            {
                case "Most Selected":
                    players = players.OrderByDescending(x => x.Player.Selections).ThenBy(x => x.Player.PlayerName).ToList();
                    break;
                case "Alphabetical":
                    players = players.OrderBy(x => x.Player.PlayerName).ToList();
                    break;
                case "Rating":
                    players = players.OrderByDescending(x => x.Player.Rating).ThenBy(x => x.Player.PlayerName).ToList();
                    break;
                default:
                    break;
            }

            ListView playerListView = FindViewById<ListView>(Resource.Id.listPlayers);
            playerListView.ChoiceMode = ChoiceMode.Multiple;
            playerListView.Adapter = new PlayerListAdapter(this, players, currentSettings.TrafficLightRatings, currentSettings.DisplayNotes, currentSettings.MaximumRating);

            FindViewById<TextView>(Resource.Id.selectedNumber).Text = "0";

            Button pickTeams = FindViewById<Button>(Resource.Id.btnPickTeams);
            ImageButton createPlayer = FindViewById<ImageButton>(Resource.Id.btnCreatePlayer);
            ImageButton matchHistory = FindViewById<ImageButton>(Resource.Id.btnMatchHistory);
            ImageButton settings = FindViewById<ImageButton>(Resource.Id.btnSettings);
            CheckBox selectAll = FindViewById<CheckBox>(Resource.Id.chkAllPlayers);

            if (players.Count > 0)
            {
                FindViewById<TextView>(Resource.Id.txtNoPlayers).Visibility = ViewStates.Invisible;
            }
            else
            {
                FindViewById<TextView>(Resource.Id.txtSelectAll).Visibility = ViewStates.Gone;
                FindViewById<TextView>(Resource.Id.chkAllPlayers).Visibility = ViewStates.Gone;
            }

            selectAll.CheckedChange += (sender, args) =>
            {
                foreach (var player in players)
                {
                    player.Selected = ((CheckBox)sender).Checked;
                    ((BaseAdapter)playerListView.Adapter).NotifyDataSetChanged();

                    FindViewById<TextView>(Resource.Id.selectedNumber).Text = players.Count(x => x.Selected).ToString();
                }
            };

            pickTeams.Click += (sender, args) =>
            {
                PlayerData selectedPlayerData = new PlayerData();
                List<Player> selectedPlayers = new List<Player>();

                for (int i = 0; i < playerListView.Count; i++)
                {
                    PlayerSelection player = playerListView.Adapter.GetItem(i).Cast<PlayerSelection>();

                    if (player.Selected)
                    {
                        selectedPlayers.Add(player.Player);
                    }
                }

                selectedPlayerData.Players = selectedPlayers.OrderByDescending(x => x.Rating).ToList();

                if (selectedPlayerData.Players.Count == 0)
                {
                    Toast.MakeText(this, "No players selected.", ToastLength.Short).Show();
                }
                else if (selectedPlayerData.Players.Count == 1)
                {
                    Toast.MakeText(this, "Need a mimimum of two players to pick teams.", ToastLength.Short).Show();
                }
                else
                {
                    string selectedPlayerDataString = selectedPlayerData.ToJSON();

                    Intent intent = new Intent(this, typeof(MatchActivity));

                    intent.PutExtra("selectedPlayerData", selectedPlayerDataString);
                    intent.PutExtra("Mode", "Create");

                    StartActivity(intent);
                }
            };

            createPlayer.Click += (sender, args) =>
            {
                Intent intent = new Intent(this, typeof(PlayerActivity));
                intent.PutExtra("Mode", "Create");

                StartActivity(intent);
            };

            matchHistory.Click += (sender, args) =>
            {
                StartActivity(typeof(MatchListActivity));
            };

            settings.Click += (sender, args) =>
            {
                StartActivity(typeof(SettingsActivity));
            };
        }
    }
}