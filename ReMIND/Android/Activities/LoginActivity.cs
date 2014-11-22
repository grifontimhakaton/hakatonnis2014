using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Android.Activities
{
    [Activity(Label = "ReMinder", MainLauncher = true, Icon = "@drawable/icon", NoHistory = true)]
    public class LoginActivity : Activity
    {
        Button btnLogin;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            bool isUserLoggedIn = false;

            if (isUserLoggedIn)
            {
                RedirectToQuestionActivity();
            }

            SetContentView(Resource.Layout.Login);

            btnLogin = (Button)FindViewById(Resource.Id.btnLogin);
            if (btnLogin != null)
            {
                btnLogin.Click += LoginUser;
            }
        }

        private void LoginUser(object sender, EventArgs e)
        {
            btnLogin.Click -= LoginUser;
            RedirectToQuestionActivity();
        }

        private void RedirectToQuestionActivity()
        {
            StartActivity(typeof(QuestionActivity));
        }
    }
}