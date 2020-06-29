using System;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Widget;
using Android.Views;

using Android.Content;

namespace GoodByeMilk.MainView {
  public class CalendarViewHolder:View{
    public TextView day_;
    public LinearLayout foodList_;

    public CalendarViewHolder(Context _context):base(_context) {

    }
  }
}
