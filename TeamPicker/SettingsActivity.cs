using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.OS;
using Android.Widget;
using Microsoft.Maui.ApplicationModel;
using System;
using TeamPicker.Classes;
using TeamPicker.Logic;

namespace TeamPicker
{
    [Activity(Theme = "@style/Theme.MyTheme")]
    public class SettingsActivity : Activity
    {
        readonly SettingsLogic sLogic = new SettingsLogic();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.Settings);

            AdView adView = FindViewById<AdView>(Resource.Id.adView);
            AdRequest adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);

            ImageButton cancelSettings = FindViewById<ImageButton>(Resource.Id.btnCancelSettings);
            Button saveSettings = FindViewById<Button>(Resource.Id.btnSaveSettings);

            SettingsData settings = sLogic.Select();
            FindViewById<TextView>(Resource.Id.numNumberOfTeams).Text = settings.NumberOfTeams.ToString();
            FindViewById<TextView>(Resource.Id.numMaxRating).Text = settings.MaximumRating.ToString();

            Switch balanceTeams = FindViewById<Switch>(Resource.Id.swBalance);
            Switch highAccuracy = FindViewById<Switch>(Resource.Id.swHighAccuracy);
            Switch trafficLightRatings = FindViewById<Switch>(Resource.Id.swTrafficLightRatings);
            Switch displayNotes = FindViewById<Switch>(Resource.Id.swDisplayNotes);

            RadioButton orderByMostSelected = FindViewById<RadioButton>(Resource.Id.rbMostSelected);
            RadioButton orderByAlphabetical = FindViewById<RadioButton>(Resource.Id.rbAlphabetical);
            RadioButton orderByRating = FindViewById<RadioButton>(Resource.Id.rbRating);

            TextView privacy = FindViewById<TextView>(Resource.Id.txtPrivacy);

            switch (settings.OrderBy)
            {
                case "Most Selected":
                    orderByMostSelected.Checked = true;
                    break;
                case "Alphabetical":
                    orderByAlphabetical.Checked = true;
                    break;
                case "Rating":
                    orderByRating.Checked = true;
                    break;
                default:
                    break;
            }

            balanceTeams.Checked = settings.BalanceTeams;
            highAccuracy.Checked = settings.HighAccuracy;
            trafficLightRatings.Checked = settings.TrafficLightRatings;
            displayNotes.Checked = settings.DisplayNotes;

            if (!settings.BalanceTeams)
            {
                highAccuracy.Enabled = false;
            }

            balanceTeams.Click += (sender, args) =>
            {
                if (!((Switch)sender).Checked)
                {
                    highAccuracy.Enabled = false;
                }
                else
                {
                    highAccuracy.Enabled = true;
                }
            };

            orderByMostSelected.Click += (sender, args) =>
            {
                settings.OrderBy = "Most Selected";
            };

            orderByAlphabetical.Click += (sender, args) =>
            {
                settings.OrderBy = "Alphabetical";
            };

            orderByRating.Click += (sender, args) =>
            {
                settings.OrderBy = "Rating";
            };

            cancelSettings.Click += (sender, args) =>
            {
                Finish();
            };

            saveSettings.Click += (sender, args) =>
            {
                string teams = FindViewById<TextView>(Resource.Id.numNumberOfTeams).Text;
                string maxRating = FindViewById<TextView>(Resource.Id.numMaxRating).Text;

                if (Convert.ToInt32(teams) > 20 || Convert.ToInt32(teams) < 2)
                {
                    Toast.MakeText(this, "Teams must be between 2 and 20.", ToastLength.Short).Show();
                }
                else if (Convert.ToInt32(maxRating) > 999 || Convert.ToInt32(maxRating) < 2)
                {
                    Toast.MakeText(this, "Max rating must be between 2 and 999.", ToastLength.Short).Show();
                }
                else
                {
                    settings.NumberOfTeams = Convert.ToInt32(teams);
                    settings.MaximumRating = Convert.ToInt32(maxRating);
                    settings.BalanceTeams = balanceTeams.Checked;
                    settings.TrafficLightRatings = trafficLightRatings.Checked;
                    settings.HighAccuracy = highAccuracy.Checked;
                    settings.DisplayNotes = displayNotes.Checked;

                    sLogic.Update(settings);

                    Toast.MakeText(this, "Settings saved.", ToastLength.Short).Show();

                    Intent intent = new Intent(this, typeof(SelectionActivity));
                    intent.SetFlags(ActivityFlags.ClearTop);
                    StartActivity(intent);
                    Finish();
                }
            };

            privacy.Click += (sender, args) =>
             {
                 Intent intent = new Intent(this, typeof(PrivacyActivity));
                 StartActivity(intent);
             };
        }
    }
}