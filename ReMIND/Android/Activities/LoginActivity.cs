using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SharedPCL.DataContracts;
using Android.Helpers;

namespace Android.Activities
{
    [Activity(Label = "ReMinder", MainLauncher = true, Icon = "@drawable/icon", NoHistory = true)]
    public class LoginActivity : Activity
    {
        Button btnLogin;
        EditText txtEmail;
        EditText txtPassword;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

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
                UserDTO currentUser = MethodHelper.LoginOrRegister(txtEmail.Text, txtPassword.Text);

                if (currentUser != null)
                {
                    btnLogin.Click -= LoginUser;
                    RedirectToQuestionActivity();
                }
                else
                {
                    //TODO Notify of error
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