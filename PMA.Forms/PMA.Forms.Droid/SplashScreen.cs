using Android.App;
using Android.Content.PM;
using MvvmCross.Droid.Views;

namespace PMA.Forms.Droid
{
    [Activity(
        Label = "PMA.Forms.Droid"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , NoHistory = true
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
        }

        private bool _isInitializationComplete;

        public override void InitializationComplete()
        {
            if (_isInitializationComplete) return;
            _isInitializationComplete = true;
            StartActivity(typeof(MvxFormsApplicationActivity));
        }

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
            Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.Forms.Forms.ViewInitialized += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.View.StyleId))
                {
                    e.NativeView.ContentDescription = e.View.StyleId;
                }
            };

            base.OnCreate(bundle);
        }
    }
}
