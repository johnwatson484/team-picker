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
using TeamPicker.Logic;
using TeamPicker.Classes;
using TeamPicker.Adapters;
using Android.Gms.Ads;

namespace TeamPicker
{
    [Activity(Theme = "@style/Theme.MyTheme")]
    public class MatchListActivity : Activity
    {
        MatchLogic mLogic = new MatchLogic();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

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