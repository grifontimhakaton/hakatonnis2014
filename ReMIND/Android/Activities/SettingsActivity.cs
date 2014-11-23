using System;
using Android.App;
using Android.Content;
using Android.Helpers;
using Android.Preferences;
using Android.Runtime;
using Android.Services;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Java.Util;

namespace Android.Activities
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class SettingsActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Settings);
        }

        private void ChangeAlarmTimes(SharedPCL.Enums.AlarmTimerType alarmTimerType)
        {
            StopService(new Intent(this, typeof(AlarmService)));
            //write
            ISharedPreferences localSettings = PreferenceManager.GetDefaultSharedPreferences(this.BaseContext);
            ISharedPreferencesEditor editor = localSettings.Edit();
            editor.PutInt("AlarmTimerType", (int) alarmTimerType);
            editor.PutBoolean("RaiseNotification", true);
            editor.Apply();
            
            StartService(new Intent(this, typeof(AlarmService)));
        }

        protected override void OnPause()
        {
            base.OnPause();
            NotificationHelper.OnPauseActivity(this.BaseContext);
        }

        protected override void OnResume()
        {
            base.OnResume();
            NotificationHelper.OnResumeActivity(this.BaseContext);
        }
    }
}

