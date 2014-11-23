using System;
using Android.Activities;
using Android.App;
using Android.Content;
using Android.Util;
using System.Threading;

namespace Android.Services
{
    [Service]
    public class SimpleService : Android.App.Service
    {
        System.Threading.Timer _timer;
        
        public override void OnStart(Android.Content.Intent intent, int startId)
        {
            base.OnStart(intent, startId);
            DoStuff();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _timer.Dispose();
        }

        public void DoStuff()
        {
            _timer = new System.Threading.Timer((o) =>
            {
                buildNotification(this);
                //Log.Debug("SimpleService", "hello from simple service");
            }
            , null, 0, 4000);
        }

        public override Android.OS.IBinder OnBind(Android.Content.Intent intent)
        {
            throw new NotImplementedException();
        }

        private static readonly int ButtonClickNotificationId = 1000;
        private int _count = 1;
        private void buildNotification(Context context)
        {
            NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            Notification.Builder builder = new Notification.Builder(context);
            Intent intent = new Intent(context, typeof(QuestionActivity));
            PendingIntent pendingIntent = PendingIntent.GetActivity(context, 0, intent, 0);

            builder
                .SetAutoCancel(true) // dismiss the notification from the notification area when the user clicks on it
                .SetContentIntent(pendingIntent) // start up this activity when the user clicks the intent.
                .SetContentTitle("ReMinded Bitch!!") // Set the title
                //.SetNumber(_count) // Display the count in the Content Info
                .SetSmallIcon(Resource.Drawable.reminder_icon) // This is the icon to display
                .SetContentText(String.Format("New questions has arrived. Snoozed {0} times.", _count))
                .SetDefaults((NotificationDefaults.Sound | NotificationDefaults.Vibrate)); // the message to display.


            //Notification notification = builder.GetNotification();
            //notificationManager.notify(R.drawable.ic_launcher, notification);
            //var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
            notificationManager.Notify(ButtonClickNotificationId, builder.Build());
        }
    }
}