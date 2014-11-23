using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Activities;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;


namespace Android.BroadcastReceivers
{
    public class NotificationSender : BroadcastReceiver
    {

        public override void OnReceive(Context context, Intent arg1)
        {
            //Toast.makeText(context, "Alarm received!", Toast.LENGTH_LONG).show();
            buildNotification(context);

        }

        public void SetAlarm(Context context)
        {
            AlarmManager am = (AlarmManager) context.GetSystemService(Context.AlarmService);
            Intent i = new Intent(context, this.Class);
            PendingIntent pi = PendingIntent.GetBroadcast(context, 0, i, 0);
            am.SetRepeating(AlarmType.ElapsedRealtimeWakeup, SystemClock.ElapsedRealtime() + 1000, 1000 , pi); // Millisec * Second * Minute
        }

        private static readonly int ButtonClickNotificationId = 1000;
        private int _count = 1;
        private void buildNotification(Context context)
        {
            NotificationManager notificationManager = (NotificationManager) context.GetSystemService(Context.NotificationService);
            Notification.Builder builder = new Notification.Builder(context);
            Intent intent = new Intent(context, typeof (QuestionActivity));
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