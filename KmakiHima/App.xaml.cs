using Android.Content;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KmakiHima
{
    public partial class App : Application
    {
        public event EventHandler OnSleepEvent;
        public event EventHandler OnResumeEvent;

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            ((MainPage)MainPage).RefreshAlertList(true);
        }

        protected override void OnSleep()
        {
            OnSleepEvent?.Invoke(this, new EventArgs());
        }

        protected override void OnResume()
        {
            OnResumeEvent?.Invoke(this, new EventArgs());
            ((MainPage)MainPage).RefreshAlertList(true);
        }
    }
}
