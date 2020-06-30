
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
using System.Linq;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;

namespace GoodByeMilk.CalendarCell {
  [Activity(Label = "CalendarCellActivity")]
  public class CalendarCellActivity : Activity {
    const int MENU_EDIT = 0x00;
    List<Util.BabyFood> foodList_;
    RecyclerView recycler_;
    CalendarCellAdapter adapter_;
    DateTime date_;
    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
      SetContentView(Resource.Layout.calendar_cell_activity);
      foodList_ = this.Intent.GetParcelableArrayListExtra("card").Cast<Util.BabyFood>().ToList();
      FindViewById<TextView>(Resource.Id.dateTimeInCell).Text = Intent.GetStringExtra("date");
      date_ = DateTime.Parse(Intent.GetStringExtra("date"));


      Android.Graphics.Point point = new Android.Graphics.Point();
      this.WindowManager.DefaultDisplay.GetSize(point);

      recycler_ = FindViewById<RecyclerView>(Resource.Id.foodList);
      adapter_ = new CalendarCellAdapter(ApplicationContext, foodList_);
      var manager_ = new LinearLayoutManager(this);
      recycler_.HasFixedSize = false;
      recycler_.SetLayoutManager(manager_);
      recycler_.LayoutParameters.Height = (int)(point.Y * 0.66667);

      recycler_.SetAdapter(adapter_);



      FindViewById<FloatingActionButton>(Resource.Id.EditFoodMenu).Click += (sender, e) => {
        var intent = new Intent(this, typeof(MenuEditor.MenuEditorActivity));
        var view = LayoutInflater.Inflate(Resource.Layout.food_list_element_edit, null);
        view.SetBackgroundColor(new Android.Graphics.Color(GetColor(Resource.Color.mintcream)));
        var popup = new PopupWindow(this);
        popup.OutsideTouchable = true;
        popup.Focusable = true;

        popup.ContentView = view;
        popup.SetBackgroundDrawable(GetDrawable(Resource.Drawable.abc_popup_background_mtrl_mult));
        popup.Width = (int)(point.X * 0.8);
        popup.Height = (int)(point.Y * 0.555555);


        var when = view.FindViewById<Spinner>(Resource.Id.editorWhen);
        when.Adapter = new ArrayAdapter(this,
            Resource.Layout.support_simple_spinner_dropdown_item,
            new string[] { "朝", "昼", "夜", "おやつ" });
        when.SetSelection(0);



        view.FindViewById<Button>(Resource.Id.enter).Click += (sender, e) => {
          var what_ = view.FindViewById<EditText>(Resource.Id.editorFoodName).Text;
          var quantStr_ = view.FindViewById<EditText>(Resource.Id.editorQuant).Text;
          var unit_ = view.FindViewById<EditText>(Resource.Id.editorUnit).Text;
          int quant_;
          int.TryParse(quantStr_, out quant_);
          foodList_.Add(new Util.BabyFood((Util.BabyFood.Kind)when.SelectedItemId, date_, what_, unit_, quant_));
          adapter_.NotifyItemInserted(foodList_.Count - 1);
          popup.Dismiss();
        };


        popup.ShowAtLocation(recycler_, GravityFlags.Center, 0, (int)(-point.Y * 0.1));
        //        StartActivityForResult(intent, MENU_EDIT);
      };

      adapter_.onClick += (holder, position) => {
        if(!holder.ItemView.Clickable) return;
        holder.ItemView.Clickable = false;
        new Handler().PostDelayed(() => holder.ItemView.Clickable = true, 500);

        var intent = new Intent(this, typeof(MenuEditor.MenuEditorActivity));
        var view = LayoutInflater.Inflate(Resource.Layout.food_list_element_edit, null);
        view.SetBackgroundColor(new Android.Graphics.Color(GetColor(Resource.Color.mintcream)));

        var popup = new PopupWindow(this);
        popup.OutsideTouchable = true;
        popup.Focusable = true;

        popup.ContentView = view;
        popup.SetBackgroundDrawable(GetDrawable(Resource.Drawable.abc_popup_background_mtrl_mult));
        popup.Width = (int)(point.X * 0.8);
        popup.Height = (int)(point.Y * 0.555555);



        var when = view.FindViewById<Spinner>(Resource.Id.editorWhen);
        when.Adapter = new ArrayAdapter(this,
            Resource.Layout.support_simple_spinner_dropdown_item,
            new string[] { "朝", "昼", "夜", "おやつ" });
        when.SetSelection((int)foodList_[position].kind_);
        view.FindViewById<EditText>(Resource.Id.editorFoodName).Text = foodList_[position].menu_;
        view.FindViewById<EditText>(Resource.Id.editorQuant).Text = foodList_[position].quantity_.ToString();
        view.FindViewById<EditText>(Resource.Id.editorUnit).Text = foodList_[position].unit_;


        view.FindViewById<Button>(Resource.Id.enter).Click += (sender, e) => {
          var what_ = view.FindViewById<EditText>(Resource.Id.editorFoodName).Text;
          var quantStr_ = view.FindViewById<EditText>(Resource.Id.editorQuant).Text;
          var unit_ = view.FindViewById<EditText>(Resource.Id.editorUnit).Text;
          int quant_;
          int.TryParse(quantStr_, out quant_);

          foodList_[position] = new Util.BabyFood((Util.BabyFood.Kind)when.SelectedItemId, date_, what_, unit_, quant_);
          adapter_.NotifyItemChanged(position);
          popup.Dismiss();
        };


        popup.ShowAtLocation(recycler_, GravityFlags.Center, 0, (int)(-point.Y * 0.1));
      };

      // Create your application here
    }


    protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data) {
      base.OnActivityResult(requestCode, resultCode, data);
      switch(requestCode) {
      case MENU_EDIT:
        var editResult = data.GetParcelableArrayListExtra("editResult").Cast<Util.BabyFood>().ToList();
        foodList_.AddRange(editResult);
        adapter_.NotifyDataSetChanged();
        break;
      default:
        break;
      }
    }

    public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e) {
      switch(keyCode) {
      case Keycode.Back:
        Intent intent = new Intent();
        intent.PutParcelableArrayListExtra("editResult", foodList_.ToArray());
        intent.PutExtra("date", Intent.GetStringExtra("date"));
        SetResult(0x00, intent);
        Finish();
        return true;
        break;
      default:
        return base.OnKeyDown(keyCode, e);
      }
    }
  }
}
