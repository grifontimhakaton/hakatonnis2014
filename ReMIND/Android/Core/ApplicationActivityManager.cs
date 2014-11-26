using System.ComponentModel;
using Android.Content;
using Android.Preferences;

namespace ReMinder.Core
{
    public enum SettingType
    {
        AlarmTimerType = 1,
        IsSettingsActive = 2,
        IsQuestionActive = 3,
        IsRegisterActive = 4,
        IsAlarmServicePaused = 5,
        IsActivityLoading = 6
    }
    public class ApplicationActivityManager
    {
        private static readonly ApplicationActivityManager _Instance = new ApplicationActivityManager();

        public static ApplicationActivityManager Instance
        {
            get { return _Instance; }
        }

        public T GetSetting<T>(Context myContext, SettingType settingKey)
        {
            var prefs = PreferenceManager.GetDefaultSharedPreferences(myContext);
            var settingValue = prefs.GetString(settingKey.ToString(), null);
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(settingValue);
        }

        public void SetSetting(Context myContext, SettingType settingType, object value)
        {
            var localSettings = PreferenceManager.GetDefaultSharedPreferences(myContext);
            var editor = localSettings.Edit();
            editor.PutString(settingType.ToString(), value.ToString());
            editor.Apply();
        }

        public bool SettingExists(Context myContext, SettingType settingType)
        {
            var localSettings = PreferenceManager.GetDefaultSharedPreferences(myContext);
            var settingExists = localSettings.Contains(settingType.ToString());

            return settingExists;
        }
    }
}
