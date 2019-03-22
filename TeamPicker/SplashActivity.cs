using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using System.Timers;
using Android.Gms.Ads;

namespace TeamPicker
{
    [Activity(Theme = "@style/Theme.MyTheme", MainLauncher = true, NoHistory = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SplashActivity : Activity
    {        
        Timer timer;        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Splash);

            MobileAds.Initialize(this, "ca-app-pub-5054611580618782~9254042917");

            StartTimer();
        }

        private void StartTimer()
        {
            timer = new Timer();
            timer.Interval = 750;
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