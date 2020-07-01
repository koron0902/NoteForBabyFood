using System;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace GoodByeMilk.CalendarCell {
  public class CalendarCellViewHolder : RecyclerView.ViewHolder {
    public LinearLayout holder_;
    public TextView when_;
    public TextView foodName_;
    public TextView quant_;
    public CalendarCellViewHolder(View _itemView) : base(_itemView) {
      holder_ = _itemView.FindViewById<LinearLayout>(Resource.Id.holder);
      when_ = _itemView.FindViewById<TextView>(Resource.Id.when);
      foodName_ = _itemView.FindViewById<TextView>(Resource.Id.foodName);
      quant_ = _itemView.FindViewById<TextView>(Resource.Id.quant);
    }
  }
}
