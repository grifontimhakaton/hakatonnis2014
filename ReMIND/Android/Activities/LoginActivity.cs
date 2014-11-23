using System;
using Android.App;
using Android.Content;
using Android.Preferences;
using Android.Runtime;
using Android.Services;
using Android.Views;
using Android.Widget;
using Android.OS;
using SharedPCL.DataContracts;
using Android.Helpers;
using Android.Content.PM;
using SharedPCL.Enums;

namespace Android.Activities
{
    [Activity(Label = "ReMinder", MainLauncher = true, NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginActivity : Activity, View.IOnTouchListener
    {
        Button btnLogin;
        EditText txtEmail;
        EditText txtPassword;

        TextView txtViewRegister;
        TextView txtViewPassword;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            RequestWindowFeature(WindowFeatures.NoTitle);

            StartNotifications(AlarmTimerType.Unknown);

            bool isUserLoggedIn = false;
            if (isUserLoggedIn)
            {
                RedirectToQuestionActivity();
            }
            else
            {
                SetContentView(Resource.Layout.Login);

                txtEmail = (EditText)FindViewById(Resource.Id.txtEmail);
                txtPassword = (EditText)FindViewById(Resource.Id.txtPassword);

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

                if (currentUser != null)
                {
                    btnLogin.Click -= LoginUser;
                    StartNotifications(AlarmTimerType.Unknown);
                    RedirectToQuestionActivity();
                }
                else
                {
                    this.RunOnUiThread(() =>
                    {
                        Toast.MakeText(this, Resource.String.ErrorWhileLogin, ToastLength.Long);
                    });
                }
            }
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
            //write
            ISharedPreferences localSettings = PreferenceManager.GetDefaultSharedPreferences(this.BaseContext);
            ISharedPreferencesEditor editor = localSettings.Edit();
            editor.PutInt("AlarmTimerType", (int)alarmTimerType);
            editor.PutBoolean("RaiseNotification", false);
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