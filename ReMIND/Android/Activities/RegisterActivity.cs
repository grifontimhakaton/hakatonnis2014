using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SharedPCL.DataContracts;
using ReMinder.Helpers;
using Android.Content.PM;
using Android.Preferences;
using SharedPCL.Enums;

namespace ReMinder.Activities
{
    [Activity(NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class RegisterActivity : Activity
    {
        Button btnLogin;
        EditText txtEmail;
        EditText txtUsername;
        EditText txtPassword;
        EditText txtRepeatPassword;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            RequestWindowFeature(WindowFeatures.NoTitle);

            bool isUserLoggedIn = false;

            if (isUserLoggedIn)
            {
                RedirectToQuestionActivity();
            }
            else
            {
                SetContentView(Resource.Layout.Register);

                txtEmail = (EditText)FindViewById(Resource.Id.txtNewEmail);
                txtUsername = (EditText)FindViewById(Resource.Id.txtNewUsername);
                txtPassword = (EditText)FindViewById(Resource.Id.txtNewPassword);
                txtRepeatPassword = (EditText)FindViewById(Resource.Id.txtRepeatPassword);

                btnLogin = (Button)FindViewById(Resource.Id.btnRegister);
                if (btnLogin != null)
                {
                    btnLogin.Click += RegisterUser;
                }
            }
        }

        private void RegisterUser(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                UserDTO currentUser = MethodHelper.LoginOrRegister(txtEmail.Text, txtUsername.Text, MD5Helper.GetMd5Hash(txtPassword.Text));

                if (currentUser != null && currentUser.Status == UserStatus.OK)
                {
                    ISharedPreferences localSettings = PreferenceManager.GetDefaultSharedPreferences(this.BaseContext);
                    ISharedPreferencesEditor editor = localSettings.Edit();
                    editor.PutInt(Helpers.Constants.USER_ID, currentUser.ID);
                    editor.Apply();

                    btnLogin.Click -= RegisterUser;
                    RedirectToQuestionActivity();
                }
                else
                {
                    this.RunOnUiThread(() =>
                    {
                        Toast.MakeText(this, Resource.String.ErrorWhileRegistration, ToastLength.Long).Show();
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

            if (this.txtUsername.Text.Length <= 0)
            {
                this.RunOnUiThread(() =>
                {
                    this.txtUsername.Error = GetString(Resource.String.UsernameValidationError);
                });

                result = false;
            }

            string password = this.txtPassword.Text;
            string repPassword = this.txtRepeatPassword.Text;

            if (password.Length <= 0)
            {
                this.RunOnUiThread(() =>
                {
                    this.txtPassword.Error = GetString(Resource.String.PasswordValidationError);
                });

                result = false;
            }

            if (repPassword.Length <= 0 || !string.Equals(password, repPassword))
            {
                this.RunOnUiThread(() =>
                {
                    this.txtRepeatPassword.Error = GetString(Resource.String.PasswordValidationError);
                });

                result = false;
            }

            if (!string.Equals(password, repPassword))
            {
                this.RunOnUiThread(() =>
                {
                    this.txtRepeatPassword.Error = GetString(Resource.String.PasswordUnMatchValidationError);
                });

                result = false;
            }

            return result;
        }

        private void RedirectToQuestionActivity()
        {
            StartActivity(typeof(QuestionActivity));
        }
    }
}