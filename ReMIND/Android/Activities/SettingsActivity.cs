﻿using System;
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
using Newtonsoft.Json;

namespace ReMinder.Activities
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class SettingsActivity : Activity
    {
        private int userId;
        int[] spinnerValues;
        List<SubjectDTO> subjectList = new List<SubjectDTO>();
        ISharedPreferences localSettings;
        private ToggleButton toggleVibrationOption;

        protected override void OnCreate(Bundle bundle)
        {
            //Core.ApplicationActivityManager.Instance.IsSettingsActive = true;
            Core.ApplicationActivityManager.Instance.SetSetting(this.BaseContext, Core.SettingType.IsSettingsActive, true);
            
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Settings);

            localSettings = PreferenceManager.GetDefaultSharedPreferences(this.BaseContext);
            userId = localSettings.GetInt(Helpers.Constants.USER_ID, 0);
            if (userId > 0)
            {
                spinnerValues = Enum.GetValues(typeof(AlarmTimerType)).Cast<AlarmTimerType>().Select(x => (int)x).ToArray();
                //var alarmTimerType = Core.ApplicationActivityManager.Instance.AlarmTimerType;
                var alarmTimerType = Core.ApplicationActivityManager.Instance.GetSetting<AlarmTimerType>(this.BaseContext, Core.SettingType.AlarmTimerType);

                int selectedPosition = Array.IndexOf(spinnerValues, (int)alarmTimerType);

                var spinnerStrings = spinnerValues.Select(x => string.Format("Every {0} mins", (int)x)).ToArray();

                Spinner spinner = FindViewById<Spinner>(Resource.Id.spinTimeOptions);
                var adapter = new ArrayAdapter<String>(this, Resource.Drawable.SpinnerTextView, spinnerStrings);
                spinner.Adapter = adapter;
                spinner.SetSelection(selectedPosition);
                spinner.ItemSelected += spinner_ItemSelected;

                subjectList = MethodHelper.GetUserSubjects(userId);
                string[] items = subjectList.Select(subject => subject.SubjectName).ToArray();

                Spinner categorySpinner = FindViewById<Spinner>(Resource.Id.spinCategoryOptions);
                //var ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItemChecked, items);
                var listAdapter = new ArrayAdapter<String>(this, Resource.Drawable.SpinnerTextView, items);
                categorySpinner.Adapter = listAdapter;
                categorySpinner.SetSelection(0);
                spinner.ItemSelected += spinner_SubjectSelected;

                toggleVibrationOption = FindViewById<ToggleButton>(Resource.Id.toggleVibrationOptions);
                toggleVibrationOption.Click += ToggleVibrationOptionButton;

                //ListView listSubjectsOptions = FindViewById<ListView>(Resource.Id.listSubjectsOptions);
                //listSubjectsOptions.ChoiceMode = Android.Widget.ChoiceMode.Multiple;
                //listSubjectsOptions.Adapter = ListAdapter;
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

        private void spinner_SubjectSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            localSettings = PreferenceManager.GetDefaultSharedPreferences(this.BaseContext);
            ISharedPreferencesEditor editor = localSettings.Edit();
            subjectList.ForEach(item => { item.UserSelected = true; });
            editor.PutString(Helpers.Constants.USER_SUBJECTS, JsonConvert.SerializeObject(subjectList));
            editor.Apply();
        }

        private void ToggleVibrationOptionButton(object sender, EventArgs e)
        {
            //toggleVibrationOption.SetBackgroundDrawable(Resources.);
        }

        private void ChangeAlarmTimes(int alarmTimerPeriods)
        {
            StopService(new Intent(this, typeof(AlarmService)));
            //Core.ApplicationActivityManager.Instance.AlarmTimerType = (AlarmTimerType)alarmTimerPeriods;
            Core.ApplicationActivityManager.Instance.SetSetting(this.BaseContext, Core.SettingType.AlarmTimerType, (AlarmTimerType)alarmTimerPeriods);
            StartService(new Intent(this, typeof(AlarmService)));
        }

        protected override void OnPause()
        {
            //Core.ApplicationActivityManager.Instance.IsActivityLoading = true;
            Core.ApplicationActivityManager.Instance.SetSetting(this.BaseContext, Core.SettingType.IsActivityLoading, true);
            base.OnPause();
            //Core.ApplicationActivityManager.Instance.IsSettingsActive = false;
            Core.ApplicationActivityManager.Instance.SetSetting(this.BaseContext, Core.SettingType.IsSettingsActive, false);
        }

        protected override void OnResume()
        {
            //Core.ApplicationActivityManager.Instance.IsSettingsActive = true;
            Core.ApplicationActivityManager.Instance.SetSetting(this.BaseContext, Core.SettingType.IsSettingsActive, true);
            base.OnResume();
            //Core.ApplicationActivityManager.Instance.IsActivityLoading = false;
            Core.ApplicationActivityManager.Instance.SetSetting(this.BaseContext, Core.SettingType.IsActivityLoading, false);
        }

        protected override void OnStop()
        {
            base.OnStop();
            //Core.ApplicationActivityManager.Instance.IsActivityLoading = false;
            Core.ApplicationActivityManager.Instance.SetSetting(this.BaseContext, Core.SettingType.IsActivityLoading, false);
        }

        protected override void OnStart()
        {
            base.OnStart();
        }
    }
}

