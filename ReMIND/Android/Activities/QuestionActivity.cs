using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;

namespace Android.Activities
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class QuestionActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Question);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.mainMenu, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            OpenSettingsActivity();

            return base.OnOptionsItemSelected(item);
        }

        public override bool OnMenuOpened(int featureId, IMenu menu)
        {
            OpenSettingsActivity();
            
            return base.OnMenuOpened(featureId, menu);
        }

        private void OpenSettingsActivity()
        {
            StartActivity(typeof(SettingsActivity));
        }
    }
}

