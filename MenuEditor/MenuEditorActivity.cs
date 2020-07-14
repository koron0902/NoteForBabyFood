
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;

namespace GoodByeMilk.MenuEditor {
  [Activity(Label = "MenuEditorActivity")]
  public class MenuEditorActivity : Activity {

    List<Util.BabyFood> foodList;
    RecyclerView recycler;
    Button addButton;
    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
      SetContentView(Resource.Layout.menu_editor_activity);


      Android.Graphics.Point point = new Android.Graphics.Point();
      this.WindowManager.DefaultDisplay.GetSize(point);
      foodList = new List<Util.BabyFood>();
      recycler = FindViewById<RecyclerView>(Resource.Id.MenuListHolder);
      var adapter = new MenuEditorAdapter(foodList);
      var manager_ = new LinearLayoutManager(this);
      recycler.HasFixedSize = false;
      recycler.SetLayoutManager(manager_);
      recycler.LayoutParameters.Height = (int)(point.Y * 0.66667);

      recycler.SetAdapter(adapter);

      addButton = FindViewById<Button>(Resource.Id.addElement);
      addButton.Click += (sender, e) => {
        foodList.Add(new Util.BabyFood(Util.BabyFood.Kind.EVENING, DateTime.Now, "", "", 0));
        adapter.NotifyDataSetChanged();
      };

      var itemTouchHelperCallback = new MenuEditorItemTouchHelper();
      itemTouchHelperCallback.onSwipe += (sender, e) => {
        var archive = new KeyValuePair<int, Util.BabyFood>(((RecyclerView.ViewHolder)sender).AdapterPosition, foodList[((RecyclerView.ViewHolder)sender).AdapterPosition]);
        foodList.RemoveAt(((RecyclerView.ViewHolder)sender).AdapterPosition);
        adapter.NotifyItemRemoved(((RecyclerView.ViewHolder)sender).AdapterPosition);
        Snackbar.Make(addButton, "qfewf", Snackbar.LengthLong).SetAction("元に戻す", (v) => {
            Android.Util.Log.Debug("afwefwaef", "asfweageaorighnpeogrerogjnerpigkm");
            foodList.Insert(archive.Key, archive.Value);
            adapter.NotifyItemInserted(archive.Key);
        }).Show();
      };


      var itemTouchHelper = new ItemTouchHelper(itemTouchHelperCallback);
      itemTouchHelper.AttachToRecyclerView(recycler);



      // Create your application here
    }

    public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e) {
      switch(keyCode) {
      case Keycode.Back:
        Intent intent = new Intent();
        intent.PutParcelableArrayListExtra("editResult", foodList.ToArray());
        SetResult(Result.Ok, intent);
        Finish();
        return true;
        break;
      default:
        return base.OnKeyDown(keyCode, e);
      }
    }
  }
}
