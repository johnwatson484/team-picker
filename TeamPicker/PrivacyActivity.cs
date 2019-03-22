using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Ads;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TeamPicker
{
    [Activity(Theme = "@style/Theme.MyTheme")]
    public class PrivacyActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Privacy);
            
            ImageButton cancelPrivacy = FindViewById<ImageButton>(Resource.Id.btnCancelPrivacy);
            TextView privacyText = FindViewById<TextView>(Resource.Id.txtPrivacyDetails);

            privacyText.Text = Policy();

            cancelPrivacy.Click += (sender, args) =>
            {
                Finish();
            };
        }

        private string Policy()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Lynx Magnus built the Team Picker app as an Ad Supported app. This service is provided by at no cost and is intended for use as is.");
            sb.AppendLine("");
            sb.AppendLine("This page is used to inform visitors regarding my policies with the collection, use, and disclosure of Personal Information if anyone decided to use the service.");
            sb.AppendLine("");
            sb.AppendLine("If you choose to use the service, then you agree to the collection and use of information in relation to this policy. The Personal Information that I collect is used for providing and improving the service. I will not use or share your information with anyone except as described in this Privacy Policy.");
            sb.AppendLine("");
            sb.AppendLine("The terms used in this Privacy Policy have the same meanings as in our Terms and Conditions, which is accessible at Team Picker unless otherwise defined in this Privacy Policy.");
            sb.AppendLine("");
            sb.AppendLine("For a better experience, while using our service, I may require you to provide us with certain personally identifiable information, including but not limited to Advertising ID. The information that I request will be retained on your device and is not collected by me in any way.");
            sb.AppendLine("");
            sb.AppendLine("The app does use third party services (Google Play Services) that may collect information used to identify you.");

            return sb.ToString();
        }
    }
}