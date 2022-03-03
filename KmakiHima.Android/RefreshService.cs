using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace KmakiHima.Droid
{
    [Service]
    public class RefreshService : Service
    {
        private NotificationCompat.Builder notificationBuilder;
        private DateTime prevNotificationTime = DateTime.MinValue;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            // From shared code or in your PCL
            notificationBuilder = new NotificationCompat.Builder(this)
                .SetSmallIcon(Resource.Drawable.notification_icon_background)
                .SetContentTitle("Kmaki hotline")
                .SetChannelId("Kmaki");

            _ = StartPolling();

            return StartCommandResult.NotSticky;
        }

        private async System.Threading.Tasks.Task StartPolling()
        {
            RestService restService = new RestService();
            NotificationManager notificationManager = (NotificationManager)GetSystemService(NotificationService);
            NotificationChannel channel = new NotificationChannel("Kmaki", "Kmaki", NotificationImportance.Default);
            Intent notificationIntent = new Intent(this, typeof(MainActivity));
            PendingIntent contentIntent = PendingIntent.GetActivity(this, 0, notificationIntent, 0);

            notificationManager.CreateNotificationChannel(channel);

            while (true)
            {
                await restService.RefreshAlertsAsync();

                IEnumerable<AlertItem> newAlerts = restService.ActiveAlerts.Where(a => a.ServerTime > prevNotificationTime);

                foreach (AlertItem newAlert in newAlerts)
                {
                    notificationBuilder = notificationBuilder.SetContentText(newAlert.Message)
                        .SetVisibility(NotificationCompat.VisibilityPublic)
                        .SetContentIntent(contentIntent);
                    notificationManager.Notify(newAlert.ID, notificationBuilder.Build());

                    prevNotificationTime = newAlert.ServerTime;
                }


                Thread.Sleep(60000);
            }
        }
    }
}