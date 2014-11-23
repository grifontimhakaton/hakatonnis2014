using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ReMinder.Helpers
{
    public static class Constants
    {
        public const string USER_ID = "UserId";
        public const string ALARM_TIMER_TYPE = "AlarmTimerType";
        public const string RAISE_NOTIFICATION = "RaiseNotification";
        public const string USER_SUBJECTS = "UserSubjects";

        public const string Digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    }
}