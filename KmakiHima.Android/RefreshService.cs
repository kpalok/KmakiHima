using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KmakiHima.Droid
{
    public class RefreshService : Service
    {
        private const int NOTIFICATION_ID = 783476;
        private Notification.Builder notificationBuilder;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            // From shared code or in your PCL
            notificationBuilder = new Notification.Builder(this)
                .SetSmallIcon(Icon.CreateWithResource(this, Resource.Drawable.notification_bg))
                .SetContentTitle("Kmaki hotline")
                .SetContentText("Uusi ilmoitus!");

            

            return StartCommandResult.NotSticky;
        }

        private void 
    }
}