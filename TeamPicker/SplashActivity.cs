using Android.App;
using Android.Gms.Ads;
using Android.OS;
using Microsoft.Maui.ApplicationModel;
using System.Timers;

namespace TeamPicker
{
    [Activity(Theme = "@style/Theme.MyTheme", MainLauncher = true, NoHistory = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SplashActivity : Activity
    {
        System.Timers.Timer timer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.Splash);

            MobileAds.Initialize(this);

            StartTimer();
        }

        private void StartTimer()
        {
            timer = new System.Timers.Timer
            {
                Interval = 750
            };
            timer.Elapsed += new ElapsedEventHandler(StartApplication);
            timer.Start();
        }

        private void StartApplication(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            StartActivity(typeof(SelectionActivity));
            Finish();
        }
    }
}