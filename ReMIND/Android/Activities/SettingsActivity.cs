using System;
using Android.App;
using Android.Content;
using ReMinder.Helpers;
using Android.Preferences;
using Android.Runtime;
using ReMinder.Services;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Java.Util;
using System.Linq;
using System.Collections.Generic;
using SharedPCL.DataContracts;
using SharedPCL.Enums;

namespace ReMinder.Activities
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class SettingsActivity : Activity
    {
        private int userId;
        int[] spinnerValues;
        List<SubjectDTO> subjectList = new List<SubjectDTO>();
        ISharedPreferences localSettings;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Settings);

            localSettings = PreferenceManager.GetDefaultSharedPreferences(this.BaseContext);
            userId = localSettings.GetInt(Helpers.Constants.USER_ID, 0);
            if (userId > 0)
            {
                spinnerValues = Enum.GetValues(typeof(AlarmTimerType)).Cast<AlarmTimerType>().Select(x => (int)x).ToArray();
                var alarmTimerType = localSettings.GetInt(Helpers.Constants.ALARM_TIMER_TYPE, (int)AlarmTimerType.None);
                int selectedPosition = Array.IndexOf(spinnerValues, alarmTimerType);

                var spinnerStrings = spinnerValues.Select(x => string.Format("Every {0} mins", (int)x)).ToArray();

                Spinner spinner = FindViewById<Spinner>(Resource.Id.spinTimeOptions);
                spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
                var adapter = new ArrayAdapter<String>(this, Resource.Drawable.SpinnerTextView, spinnerValues);
                spinner.Adapter = adapter;
                spinner.SetSelection(selectedPosition);

                subjectList = MethodHelper.GetUserSubjects(userId);
                string[] items = subjectList.Select(subject => subject.SubjectName).ToArray();

                var ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItemChecked, items);

                ListView listSubjectsOptions = FindViewById<ListView>(Resource.Id.listSubjectsOptions);
                listSubjectsOptions.ChoiceMode = Android.Widget.ChoiceMode.Multiple;
                listSubjectsOptions.Adapter = ListAdapter;
            }
            else
            {
                StartActivity(typeof(LoginActivity));
            }
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ChangeAlarmTimes(spinnerValues[e.Position]);
        }

        private void ChangeAlarmTimes(int alarmTimerPeriods)
        {
            StopService(new Intent(this, typeof(AlarmService)));
            //write
            localSettings = PreferenceManager.GetDefaultSharedPreferences(this.BaseContext);
            ISharedPreferencesEditor editor = localSettings.Edit();
            editor.PutInt(Helpers.Constants.ALARM_TIMER_TYPE, alarmTimerPeriods);
            editor.PutBoolean(Helpers.Constants.RAISE_NOTIFICATION, true);
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

