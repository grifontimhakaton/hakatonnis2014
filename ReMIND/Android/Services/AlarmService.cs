using System;
using ReMinder.Activities;
using Android.App;
using Android.Content;
using Android.Preferences;
using SharedPCL.Enums;
using System.Collections.Generic;
using SharedPCL.DataContracts;
using Newtonsoft.Json;

namespace ReMinder.Services
{
    [Service]
    public class AlarmService : Android.App.Service
    {
        private System.Threading.Timer _timer;
        private bool RaiseNotification { get; set; }

        public override void OnStart(Android.Content.Intent intent, int startId)
        {
            base.OnStart(intent, startId);
            DoStuff();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (_timer != null)
            {
                _timer.Dispose();
            }
        }

        public void DoStuff()
        {
            //read prefs 
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.BaseContext);
            var alarmTimerType = prefs.GetInt(Helpers.Constants.ALARM_TIMER_TYPE, (int)AlarmTimerType.None);
            RemindedTimes = 0;

            var time = GetTime((AlarmTimerType) alarmTimerType);
            if (time > 0)
            {
                _timer = new System.Threading.Timer((o) =>
                {
                    buildNotification(this);
                }, null, 0, time);
            }
        }

        private int GetTime(AlarmTimerType alarmTimerType)
        {
            return (int)alarmTimerType * 60 * 1000;
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
            RemindedTimes++;
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.BaseContext);
            RaiseNotification = prefs.GetBoolean(Helpers.Constants.RAISE_NOTIFICATION, false);
            var alarmTimerType = (AlarmTimerType)prefs.GetInt(Helpers.Constants.ALARM_TIMER_TYPE, (int)AlarmTimerType.None);
            var isFromLoging = prefs.GetBoolean("IsFromLogin", false);
            if (!RaiseNotification || alarmTimerType == AlarmTimerType.None || isFromLoging)
            {
                return;
            }

            var subject = prefs.GetString(Helpers.Constants.USER_SUBJECTS, "More");
            try
            {
                List<SubjectDTO> userSubjectList = JsonConvert.DeserializeObject<List<SubjectDTO>>(subject);
                SubjectDTO randomUserSubject = userSubjectList.Find(item => item.UserSelected);
                if (randomUserSubject != null)
                {
                    subject = randomUserSubject.SubjectName;
                }
            }
            catch (Exception ex)
            {
            }

            var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
            var builder = new Notification.Builder(context);
            var intent = new Intent(context, typeof(LoginActivity));
            var pendingIntent = PendingIntent.GetActivity(context, 0, intent, 0);

            builder
                .SetAutoCancel(true) // dismiss the notification from the notification area when the user clicks on it
                .SetContentIntent(pendingIntent) // start up this activity when the user clicks the intent.
                .SetContentTitle("ReMind") // Set the title
                .SetNumber(RemindedTimes) // Display the count in the Content Info
                .SetSmallIcon(Resource.Drawable.reminder_icon) // This is the icon to display
                .SetContentText(String.Format("New question from subject: {0}", subject))
                .SetDefaults((NotificationDefaults.Sound | NotificationDefaults.Vibrate | NotificationDefaults.Lights)); // the message to display.

            notificationManager.Notify(ButtonClickNotificationId, builder.Build());
            RemindedTimes = 0;
        }

        private bool isMyServiceRunning(AlarmService serviceClass)
        {
            ActivityManager manager = (ActivityManager) GetSystemService(Context.ActivityService);

            foreach (var service in manager.GetRunningServices(int.MaxValue))
            {
                if (serviceClass.Class.Name == (service.Service.ClassName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
