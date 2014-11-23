using System;
using Android.Activities;
using Android.App;
using Android.Content;
using Android.Preferences;
using SharedPCL.Enums;

namespace Android.Services
{
    [Service]
    public class AlarmService : Android.App.Service
    {
        System.Threading.Timer _timer;
        private bool RaiseNotification { get; set; }

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
            //read prefs 
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences (this.BaseContext);
            var alarmTimerType = prefs.GetInt(Helpers.Constants.ALARM_TIMER_TYPE, 1);
            RemindedTimes = 0;


            var time = GetTime((AlarmTimerType) alarmTimerType);
            _timer = new System.Threading.Timer((o) =>
            {
                buildNotification(this);
                //Log.Debug("SimpleService", "hello from simple service");
            }
            , null, 0, time);
        }
        
        private int GetTime(AlarmTimerType alarmTimerType)
        {
            switch (alarmTimerType)
            {
                case AlarmTimerType.Five:
                    return 1000*60*5;
                case AlarmTimerType.Fifteen:
                    return 1000*60*15;
                case AlarmTimerType.Sixty:
                    return 1000*60*60;
                case AlarmTimerType.Ten:
                    return 1000*60*10;
                case AlarmTimerType.Thirdy:
                    return 1000*60*30;
                case AlarmTimerType.Twenty:
                    return 1000*60*20;
                default:
                    return 1000*5;
            }
        }

        public override Android.OS.IBinder OnBind(Android.Content.Intent intent)
        {
            //throw new NotImplementedException();
            return null;
        }

        private static readonly int ButtonClickNotificationId = 1000;
        private static int RemindedTimes { get; set; }
        private void buildNotification(Context context)
        {
            RemindedTimes ++;
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.BaseContext);
            RaiseNotification = prefs.GetBoolean(Helpers.Constants.RAISE_NOTIFICATION, false);
            var alarmTimerType = (AlarmTimerType)prefs.GetInt(Helpers.Constants.ALARM_TIMER_TYPE, 1);
            if (!RaiseNotification || alarmTimerType == AlarmTimerType.None)
            {
                return;
            }

            var category = prefs.GetString("Category", "Test");
            var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            var builder = new Notification.Builder(context);
            var intent = new Intent(context, typeof(QuestionActivity));
            var pendingIntent = PendingIntent.GetActivity(context, 0, intent, 0);

            builder
                .SetAutoCancel(true) // dismiss the notification from the notification area when the user clicks on it
                .SetContentIntent(pendingIntent) // start up this activity when the user clicks the intent.
                .SetContentTitle("ReMinded Bitch!!") // Set the title
                .SetNumber(RemindedTimes) // Display the count in the Content Info
                .SetSmallIcon(Resource.Drawable.reminder_icon) // This is the icon to display
                .SetContentText(String.Format("New question from category: {0}", category))
                .SetDefaults((NotificationDefaults.Sound | NotificationDefaults.Vibrate)); // the message to display.


            //Notification notification = builder.GetNotification();
            //notificationManager.notify(R.drawable.ic_launcher, notification);
            //var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
            notificationManager.Notify(ButtonClickNotificationId, builder.Build());
            RemindedTimes = 0;
        }
    }
}