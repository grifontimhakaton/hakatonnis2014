using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Android.Activities
{
    [Activity]
    public class QuestionActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Question);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.mainmenu, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    

        //    //var shareMenuItem = menu.FindItem(Resource.Id.shareMenuItem);
        //    //var shareActionProvider =
        //    //   (ShareActionProvider)shareMenuItem.ActionProvider;
        //    //shareActionProvider.SetShareIntent(CreateIntent());
        //    return true;
        //}

        public override bool OnMenuOpened(int featureId, IMenu menu)
        {
            StartActivity(typeof(SettingsActivity));
            return base.OnMenuOpened(featureId, menu);
        }

        private void OpenSettingsActivity(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Tada", ToastLength.Short);
        }
    }
}

