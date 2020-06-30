using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.Linq;
using Android.Content;

namespace GoodByeMilk {
  [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
  public class MainActivity : AppCompatActivity {
    private TextView titleText;
    private Button prevButton, nextButton;
    private MainView.CalendarAdapter mCalendarAdapter;
    private GridView calendarGridView;
    Util.DataManager dataManager_;

    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
      Xamarin.Essentials.Platform.Init(this, savedInstanceState);
      SetContentView(Resource.Layout.activity_main);


      var titleText = FindViewById<TextView>(Resource.Id.titleText);
      var prevButton = FindViewById<Button>(Resource.Id.prevButton);
      prevButton.Click += (sender, e) => {
        mCalendarAdapter.prevMonth();
        titleText.Text = mCalendarAdapter.getTitle();
      };

      var nextButton = FindViewById<Button>(Resource.Id.nextButton);
      nextButton.Click += (sender, e) => {
        mCalendarAdapter.nextMonth();
        titleText.Text = mCalendarAdapter.getTitle();

      };

      dataManager_ = new Util.DataManager(ApplicationContext);

      calendarGridView = FindViewById<GridView>(Resource.Id.calendarGridView);
      mCalendarAdapter = new MainView.CalendarAdapter(this, dataManager_.dataRef_);
      calendarGridView.Adapter = mCalendarAdapter;


      calendarGridView.ItemClick += (sender, e) => {
        var selectedDate = DateTime.Parse((string)((GridView)sender).GetItemAtPosition(e.Position));

        var foodList = dataManager_.dataRef_.Where(elm => elm.date_.Date == selectedDate);
        var intent = new Android.Content.Intent(this, typeof(CalendarCell.CalendarCellActivity));
        intent.PutExtra("date", selectedDate.ToString("yyyy年MM月dd日"));
        intent.PutParcelableArrayListExtra("card", foodList.ToArray());

        StartActivityForResult(intent, 0x00);
      };

      titleText.Text = mCalendarAdapter.getTitle();
    }

    public override bool OnCreateOptionsMenu(IMenu menu) {
      MenuInflater.Inflate(Resource.Menu.menu_main, menu);
      return true;
    }

    public override bool OnOptionsItemSelected(IMenuItem item) {
      int id = item.ItemId;
      if(id == Resource.Id.action_settings) {
        return true;
      }

      return base.OnOptionsItemSelected(item);
    }

    private void FabOnClick(object sender, EventArgs eventArgs) {
      View view = (View)sender;
      Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
          .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
    }
    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults) {
      Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

      base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data) {
      base.OnActivityResult(requestCode, resultCode, data);
      var editResult = data.GetParcelableArrayListExtra("editResult").Cast<Util.BabyFood>().ToList();
      var date = DateTime.Parse(data.GetStringExtra("date"));

      var origData = dataManager_.dataRef_.Where(elm => elm.date_.Date == date);

      var removedData = origData.Where(elm => editResult.Where(mle => elm.hash_ == mle.hash_).Count() == 0).ToList();
      var newData = editResult.Where(elm => origData.Where(mle => elm.hash_ == mle.hash_).Count() == 0).ToList();

      dataManager_.AddData(newData);
      dataManager_.RemoveData(removedData);
      mCalendarAdapter.NotifyDataSetChanged();

    }
  }
}

