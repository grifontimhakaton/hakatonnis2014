using SharedPCL.Enums;

namespace ReMinder.Core
{
    public class ApplicationActivityManager
    {
        private static ApplicationActivityManager _instance;
        public static bool IsSettingsActive { get; set; }
        public static bool IsQuestionActive { get; set; }
        public static bool IsRegisterActive { get; set; }
        public static AlarmTimerType AlarmTimerType { get; set; }
        public static bool IsAlarmServicePaused { get; set; }
        public static bool IsActivityLoading { get; set; }


        private ApplicationActivityManager()
        {
            AlarmTimerType = AlarmTimerType.None;
        }

        public static ApplicationActivityManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ApplicationActivityManager();
                }
                return _instance;
            }
        }
    }
}