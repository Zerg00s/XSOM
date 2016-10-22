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
using o365Auth;

namespace XSOM.Adapters
{
    public class CheckoutsAdapter: BaseAdapter<XListItem>
    {
        List<XListItem> items;
        Activity context;

        public CheckoutsAdapter(Activity activity, List<XListItem> items): base()
        {
            this.items = items;
            this.context = activity;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override XListItem this[int position]
        {
            get
            {
                return items[position];
            }
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            if (convertView == null)
            {
             // convertView = this.context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleExpandableListItem1);
            }

            // convertView.FindViewById<TextView>()
            return null;
        }
    }
}