using System;
using System.Linq;
using Xamarin.Forms;

namespace KmakiHima
{
    public partial class MainPage : ContentPage
    {
        private AlertItem activeAlert;
        public RestService RestService { get; private set; }

        public MainPage()
        {
            InitializeComponent();

            RestService = new RestService();
        }

        public async void RefreshAlertList(bool refreshService = false)
        {
            if (refreshService)
            {
                await RestService.RefreshAlertsAsync();
            }

            lblQueue.Text = $"{RestService.ActiveAlerts?.Count ?? 0} viestiä jonossa";
            activeAlert = RestService.ActiveAlerts?.OrderByDescending(a => a.ServerTime).FirstOrDefault();

            if (activeAlert == null)
            {
                slActiveAlert.IsVisible = false;
                btnRefresh.IsVisible = true;
            }
            else
            {
                slActiveAlert.IsVisible = true;
                btnRefresh.IsVisible = false;
                lblTimeStamp.Text = $"Lähetetty {activeAlert.ServerTime:MM.dd HH.mm}";
                eMessage.Text = activeAlert.Message;
            }
        }

        private async void btnAccept_Clicked(object sender, EventArgs e)
        {
            activeAlert.Approved = true;
            await RestService.UpdateAlertStatus(activeAlert);
            RefreshAlertList();
        }

        private async void btnDecline_Clicked(object sender, EventArgs e)
        {
            activeAlert.Declined = true;
            await RestService.UpdateAlertStatus(activeAlert);
            RefreshAlertList();
        }

        private void btnRefresh_Clicked(object sender, EventArgs e)
        {
            RefreshAlertList(true);
        }
    }
}
