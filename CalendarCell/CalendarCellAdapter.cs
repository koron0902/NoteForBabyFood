using System;
using Android.Support.V7.Widget;
using Android.Views;
using System.Collections.Generic;
using Android.Content;
using System.Reflection;
using Android.OS;

namespace GoodByeMilk.CalendarCell {
  public class CalendarCellAdapter : RecyclerView.Adapter {
    List<Util.BabyFood> foodList_;
    RecyclerView recycler_;
    public Action<int> onClick;
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

      holder.ItemView.Click -= ItemView_Click;
      holder.ItemView.Click += ItemView_Click;

      ((CalendarCellViewHolder)holder).ItemView.SetBackgroundColor(new Android.Graphics.Color(context_.GetColor(Resource.Color.powderblue)));
    }

    private void ItemView_Click(object sender, EventArgs e) {
      var position = recycler_.GetChildAdapterPosition((View)sender);
      onClick.Invoke(position);
    }

    public override void OnAttachedToRecyclerView(RecyclerView recyclerView) {
      base.OnAttachedToRecyclerView(recyclerView);
      recycler_ = recyclerView;
    }

    public override void OnDetachedFromRecyclerView(RecyclerView recyclerView) {
      base.OnDetachedFromRecyclerView(recyclerView);
      recycler_ = null;
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
