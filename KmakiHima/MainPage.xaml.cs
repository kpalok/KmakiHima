using System;
using System.Linq;
using Xamarin.Forms;

namespace KmakiHima
{
    public partial class MainPage : ContentPage
    {
        private RestService restService;
        private AlertItem activeAlert;

        public MainPage()
        {
            InitializeComponent();

            restService = new RestService();
        }

        public async void RefreshAlertList(bool refreshService = false)
        {
            if (refreshService)
            {
                await restService.RefreshAlertsAsync();
            }

            lblQueue.Text = $"{restService.ActiveAlerts?.Count ?? 0} viestiä jonossa";
            activeAlert = restService.ActiveAlerts?.OrderByDescending(a => a.ServerTime).FirstOrDefault();

            if (activeAlert == null)
            {
                slActiveAlert.IsVisible = false;
            }
            else
            {
                slActiveAlert.IsVisible = true;
                lblTimeStamp.Text = $"Lähetetty {activeAlert.ServerTime:MM.dd HH.mm}";
                eMessage.Text = activeAlert.Message;
            }
        }

        private async void btnAccept_Clicked(object sender, EventArgs e)
        {
            activeAlert.Approved = true;
            await restService.UpdateAlertStatus(activeAlert);
            RefreshAlertList();
        }

        private async void btnDecline_Clicked(object sender, EventArgs e)
        {
            activeAlert.Declined = true;
            await restService.UpdateAlertStatus(activeAlert);
            RefreshAlertList();
        }
    }
}
