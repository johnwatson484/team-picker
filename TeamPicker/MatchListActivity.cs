using Android.App;
using Android.Gms.Ads;
using Android.OS;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.ApplicationModel;
using System.Collections.Generic;
using TeamPicker.Adapters;
using TeamPicker.Classes;
using TeamPicker.Logic;

namespace TeamPicker
{
    [Activity(Theme = "@style/Theme.MyTheme")]
    public class MatchListActivity : Activity
    {
        readonly MatchLogic mLogic = new MatchLogic();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.MatchList);

            AdView adView = FindViewById<AdView>(Resource.Id.adView);
            AdRequest adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);

            List<Match> matches = mLogic.SelectAll();

            ListView matchListView = FindViewById<ListView>(Resource.Id.listMatches);

            matchListView.Adapter = new MatchListAdapter(this, matches);

            FindViewById<TextView>(Resource.Id.matchNumber).Text = matches.Count.ToString();

            ImageButton cancelMatchList = FindViewById<ImageButton>(Resource.Id.btnCancelMatchList);
            TextView noMatches = FindViewById<TextView>(Resource.Id.txtNoMatches);

            if (matches.Count > 0)
            {
                noMatches.Visibility = ViewStates.Invisible;
            }

            cancelMatchList.Click += (sender, args) =>
            {
                Finish();
            };
        }
    }
}