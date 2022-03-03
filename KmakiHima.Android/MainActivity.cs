using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Content;

namespace KmakiHima.Droid
{
    [Activity(Label = "KmakiHima", Icon = "@drawable/doorbell", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private Intent intent;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            App app = new App();
            LoadApplication(app);

            app.OnSleepEvent += App_OnSleepEvent;
            app.OnResumeEvent += App_OnResumeEvent;

            intent = new Intent(this, typeof(RefreshService));
        }

        private void App_OnSleepEvent(object sender, EventArgs e)
        {
            _ = StartService(intent);
        }

        private void App_OnResumeEvent(object sender, EventArgs e)
        {
            _ = StopService(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}