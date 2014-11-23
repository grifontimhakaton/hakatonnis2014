using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SharedPCL.DataContracts;
using Android.Helpers;
using Android.Content.PM;

namespace Android.Activities
{
    [Activity(NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class RegisterActivity : Activity
    {
        Button btnLogin;
        EditText txtEmail;
        EditText txtPassword;

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

                txtEmail = (EditText)FindViewById(Resource.Id.txtEmail);
                txtPassword = (EditText)FindViewById(Resource.Id.txtPassword);

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
    }
}