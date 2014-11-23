using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Android.Helpers
{
    class UserData
    {
        public static readonly UserData _instance = new UserData();

        public int UserId { get; set; }

        public void Test()
        {
            // Code runs.
        }

        UserData()
        {
        }
    }
}