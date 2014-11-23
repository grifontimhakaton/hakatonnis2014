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
using SharedPCL.DataContracts;

namespace ReMinder.Adapters
{
    public class AnswerAdapter : BaseAdapter<string>
    {
        string[] items;
        Activity context;
        public AnswerAdapter(Activity context, string[] items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override string this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Length; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var vi = convertView;
            if (vi == null)
            {
                vi = context.LayoutInflater.Inflate(Resource.Layout.CustomRowView, null);
            }
            TextView text = (TextView)vi.FindViewById(Resource.Id.questionRowText);
            text.Text = items[position];
            return vi;
            
            
            
            
            
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItemChecked, null);
            // set view properties to reflect data for the given row
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = items[position];
            // return the view, populated with data, for display
            return view;
        }
    }
}
