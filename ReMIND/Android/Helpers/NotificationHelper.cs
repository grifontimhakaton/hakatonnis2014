using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Android.Helpers
{
    public static class NotificationHelper
    {
        public static void OnResumeActivity(Context context)
        {
            ISharedPreferences localSettings = PreferenceManager.GetDefaultSharedPreferences(context);
            ISharedPreferencesEditor editor = localSettings.Edit();
            editor.PutBoolean("RaiseNotification", false);
            editor.Apply();
        }

        public static void OnPauseActivity(Context context)
        {
            ISharedPreferences localSettings = PreferenceManager.GetDefaultSharedPreferences(context);
            ISharedPreferencesEditor editor = localSettings.Edit();
            editor.PutBoolean("RaiseNotification", true);
            editor.Apply();
        }
    }
}