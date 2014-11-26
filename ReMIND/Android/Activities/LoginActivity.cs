using System;
using Android.App;
using Android.Content;
using Android.Preferences;
using ReMinder.Services;
using Android.Views;
using Android.Widget;
using Android.OS;
using SharedPCL.DataContracts;
using ReMinder.Helpers;
using Android.Content.PM;
using SharedPCL.Enums;

namespace ReMinder.Activities
{
    [Activity(Label = "ReMinder", MainLauncher = true, NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginActivity : Activity, View.IOnTouchListener
    {
        Button btnLogin;
        EditText txtEmail;
        EditText txtPassword;

        TextView txtViewRegister;
        TextView txtViewPassword;

        ISharedPreferences localSettings;

        protected override void OnCreate(Bundle bundle)
        {
            //Core.ApplicationActivityManager.Instance.IsQuestionActive = true; // HACK!! Because of first activity
            Core.ApplicationActivityManager.Instance.SetSetting(this.BaseContext, Core.SettingType.IsQuestionActive, true);
            base.OnCreate(bundle);

            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            RequestWindowFeature(WindowFeatures.NoTitle);

            if (IsUserLoggedIn())
            {
                RedirectToQuestionActivity();
                StartNotifications(AlarmTimerType.None);
            }
            else
            {
                SetContentView(Resource.Layout.Login);

                txtEmail = (EditText)FindViewById(Resource.Id.txtEmail);
                txtEmail.Text = "test@email.com";
                txtPassword = (EditText)FindViewById(Resource.Id.txtPassword);
                txtPassword.Text = "123456aaa";

                txtViewRegister = (TextView)FindViewById(Resource.Id.txtViewRegister);
                txtViewPassword = (TextView)FindViewById(Resource.Id.txtViewPassword);

                txtViewRegister.SetOnTouchListener(this);
                //txtViewPassword.SetOnTouchListener(this);

                btnLogin = (Button)FindViewById(Resource.Id.btnLogin);
                if (btnLogin != null)
                {
                    btnLogin.Click += LoginUser;
                }
            }
        }

        private void LoginUser(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                UserDTO currentUser = MethodHelper.LoginOrRegister(txtEmail.Text, MD5Helper.GetMd5Hash(txtPassword.Text));

                if (currentUser != null && currentUser.Status == UserStatus.OK)
                {
                    localSettings = PreferenceManager.GetDefaultSharedPreferences(this.BaseContext);
                    ISharedPreferencesEditor editor = localSettings.Edit();
                    editor.PutInt(Helpers.Constants.USER_ID, currentUser.ID);
                    editor.Apply();

                    btnLogin.Click -= LoginUser;
                    RedirectToQuestionActivity();
                    StartNotifications(AlarmTimerType.None);
                }
                else
                {
                    this.RunOnUiThread(() =>
                    {
                        Toast.MakeText(this, Resource.String.ErrorWhileLogin, ToastLength.Long).Show();
                    });
                }
            }
        }

        private bool IsUserLoggedIn()
        {
            localSettings = PreferenceManager.GetDefaultSharedPreferences(this.BaseContext);
            return localSettings.GetInt(Helpers.Constants.USER_ID, 0) > 0;
            //return true;
        }

        private bool ValidateFields()
        {
            bool result = true;

            if (this.txtEmail.Text.Length <= 0)
            {
                this.RunOnUiThread(() =>
                {
                    this.txtEmail.Error = GetString(Resource.String.EmailValidationError);
                });

                result = false;
            }

            if (this.txtPassword.Text.Length <= 0)
            {
                this.RunOnUiThread(() =>
                {
                    this.txtPassword.Error = GetString(Resource.String.PasswordValidationError);
                });
                result = false;
            }

            return result;
        }

        private void RedirectToQuestionActivity()
        {
            StartActivity(typeof(QuestionActivity));
        }

        bool View.IOnTouchListener.OnTouch(View v, MotionEvent e)
        {
            if (v.Id == Resource.Id.txtViewRegister)
            {
                StartActivity(typeof(RegisterActivity));
            }
            //else if (viewId == Resource.Id.txtViewPassword)
            //{
            //    RunOnUiThread(() =>
            //        {
            //            Toast.MakeText(this, Resource.String.NotImplemented, ToastLength.Long);
            //        });
            //}
            return true;

        }

        private void StartNotifications(AlarmTimerType alarmTimerType)
        {
            StopService(new Intent(this, typeof(AlarmService)));
            if (!Core.ApplicationActivityManager.Instance.SettingExists(this.BaseContext, Core.SettingType.AlarmTimerType))
            {
                Core.ApplicationActivityManager.Instance.SetSetting(this.BaseContext, Core.SettingType.AlarmTimerType, AlarmTimerType.None);
            }

            StartService(new Intent(this, typeof(AlarmService)));
        }

        //protected override void OnPause()
        //{
        //    base.OnPause();
        //    NotificationHelper.OnPauseActivity(this.BaseContext);
        //}

        //protected override void OnResume()
        //{
        //    base.OnResume();
        //    NotificationHelper.OnResumeActivity(this.BaseContext);
        //}
    }
}