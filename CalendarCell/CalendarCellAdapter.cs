using System;
using Android.Support.V7.Widget;
using Android.Views;
using System.Collections.Generic;
using Android.Content;

namespace GoodByeMilk.CalendarCell {
  public class CalendarCellAdapter : RecyclerView.Adapter {
    List<Util.BabyFood> foodList_;
    public Action<RecyclerView.ViewHolder, int> onClick;
    Context context_;
    public CalendarCellAdapter(Context _context, List<Util.BabyFood> _data) {
      foodList_ = _data;
      context_ = _context;
    }

    public override int ItemCount => foodList_.Count;

    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
      ((CalendarCellViewHolder)holder).when_.Text = Util.BabyFood.ToString(foodList_[position].kind_);
      ((CalendarCellViewHolder)holder).foodName_.Text = foodList_[position].menu_;
      ((CalendarCellViewHolder)holder).quant_.Text = foodList_[position].quantity_.ToString() + "(" + foodList_[position].unit_ + ")";
      switch(foodList_[position].kind_) {
      case Util.BabyFood.Kind.MONING:
        ((CalendarCellViewHolder)holder).ItemView.SetBackgroundColor(new Android.Graphics.Color(context_.GetColor(Resource.Color.skyblue)));
        break;
      case Util.BabyFood.Kind.NOON:
        ((CalendarCellViewHolder)holder).ItemView.SetBackgroundColor(new Android.Graphics.Color(context_.GetColor(Resource.Color.paleturquoise)));
        break;
      case Util.BabyFood.Kind.EVENING:
        ((CalendarCellViewHolder)holder).ItemView.SetBackgroundColor(new Android.Graphics.Color(context_.GetColor(Resource.Color.powderblue)));
        break;
      case Util.BabyFood.Kind.SNACK:
        ((CalendarCellViewHolder)holder).ItemView.SetBackgroundColor(new Android.Graphics.Color(context_.GetColor(Resource.Color.lightblue)));
        break;

      }

      holder.ItemView.Click += (sender, e) => {
        onClick(holder, position);
      };
    }

    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
      return new CalendarCellViewHolder(LayoutInflater
      .From(parent.Context)
      .Inflate(Resource.Layout.food_list_element,
          parent,
          false));
    }
  }
}
