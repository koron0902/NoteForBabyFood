using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Android.Runtime;
using System.Linq;

namespace GoodByeMilk.MainView {
  public class CalendarAdapter : BaseAdapter {
    private List<DateTime> dateArray = new List<DateTime>();
    public IReadOnlyList<DateTime> dateArrayRef;
    private Context mContext;
    private Util.DateManager dateManager_;
    private LayoutInflater mLayoutInflater;
    private IReadOnlyList<Util.BabyFood> data_;

    public override int Count => dateArray.Count;

    public CalendarAdapter(Context context, IReadOnlyList<Util.BabyFood> _data) {
      mContext = context;
      data_ = _data;
      mLayoutInflater = LayoutInflater.From(mContext);
      dateManager_ = new Util.DateManager();
      dateArray = dateManager_.getDays();
      dateArrayRef = dateArray;
    }



    public override View GetView(int position, View convertView, ViewGroup parent) {
      CalendarViewHolder holder;
      if(convertView == null) {
        convertView = mLayoutInflater.Inflate(Resource.Layout.calendar_cell, null);
        holder = new CalendarViewHolder(mContext);
        holder.day_ = convertView.FindViewById<TextView>(Resource.Id.dateText);
        holder.foodList_ = convertView.FindViewById<LinearLayout>(Resource.Id.menu_in_cell);
        convertView.Tag = holder;
      } else {
        holder = (CalendarViewHolder)convertView.Tag;
      }

      holder.foodList_.RemoveAllViews();
      var count = 0;
      foreach(var m in data_.Where(elm => elm.date_ == dateArray[position]).ToList()) {
        if(count++ > 4)
          break;
        var tv = new TextView(mContext);
        if(m.menu_.Length > 5) tv.Text = m.menu_.Substring(0, 4) + "..";
        else tv.Text = m.menu_;
        tv.SetTextSize(Android.Util.ComplexUnitType.Dip, 12);
        tv.SetTextColor(new Android.Graphics.Color(mContext.GetColor(Resource.Color.cyan)));
        holder.foodList_.AddView(tv);
      }



      /*convertView.Click += (sender, e) => {
        Toast.MakeText(mContext, holder.day_.Text, ToastLength.Long).Show();
        Toast.MakeText(mContext, position.ToString(), ToastLength.Long).Show();
        var a = dataManager_.dataRef_.Where(elm => elm.date_.Date == dateArray[position]);
        var intent = new Intent(mContext, typeof(CalendarCell.CalendarCellActivity));
        intent.PutExtra("date", dateArray[position].ToString("yyyy年MM月dd日"));
        intent.PutParcelableArrayListExtra("card", a.ToArray()); // intent に Card を詰める

        mContext.StartActivity(intent);
      };*/

      //セルのサイズを指定
      float dp = mContext.Resources.DisplayMetrics.Density;
      AbsListView.LayoutParams layoutParams = new AbsListView.LayoutParams(parent.Width / 7 - (int)dp, (parent.Height - (int)dp * dateManager_.getWeeks()) / dateManager_.getWeeks());
      convertView.LayoutParameters = layoutParams;

      //日付のみ表示させる
      holder.day_.Text = dateArray[position].Day.ToString();

      //当月以外のセルをグレーアウト
      if(dateManager_.isCurrentMonth(dateArray[position])) {
        convertView.SetBackgroundColor(new Android.Graphics.Color(mContext.GetColor(Resource.Color.white)));
      } else {
        convertView.SetBackgroundColor(new Android.Graphics.Color(mContext.GetColor(Resource.Color.dimgray)));
      }

      //日曜日を赤、土曜日を青に
      int colorId;
      switch(dateManager_.getDayOfWeek(dateArray[position])) {
      case 0:
        colorId = Resource.Color.red;
        break;
      case 6:
        colorId = Resource.Color.blue;
        break;

      default:
        colorId = Resource.Color.black;
        break;
      }
      holder.day_.SetTextColor(new Android.Graphics.Color(mContext.GetColor(colorId)));

      return convertView;
    }

    //表示月を取得
    public System.String getTitle() {
      return dateManager_.date_.ToString("yyyy.MM");
    }

    //翌月表示
    public void nextMonth() {
      dateManager_.nextMonth();
      dateArray = dateManager_.getDays();
      this.NotifyDataSetChanged();

      dateArrayRef = dateArray;
    }

    //前月表示
    public void prevMonth() {
      dateManager_.prevMonth();
      dateArray = dateManager_.getDays();
      this.NotifyDataSetChanged();

      dateArrayRef = dateArray;
    }

    public override Java.Lang.Object GetItem(int position) {
      return dateArray[position].ToString("yyyy-MM-dd");
    }

    public override long GetItemId(int position) {
      return 0;
    }
  }
}