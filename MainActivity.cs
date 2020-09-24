using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace GoodByeMilk {
  [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
  public class MainActivity : AppCompatActivity {
    private TextView titleText;
    private Button prevButton, nextButton;
    private MainView.CalendarAdapter mCalendarAdapter;
    private GridView calendarGridView;
    Util.DataManager dataManager_;
    Intent intent_;

    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);

      if(!checkPermission(true)) {
        requestPermission(true);
      }

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

      dataManager_ = new Util.DataManager(this);

      calendarGridView = FindViewById<GridView>(Resource.Id.calendarGridView);
      mCalendarAdapter = new MainView.CalendarAdapter(this, dataManager_.dataRef_);
      calendarGridView.Adapter = mCalendarAdapter;


      calendarGridView.ItemClick += (sender, e) => {
        var selectedDate = DateTime.Parse((string)((GridView)sender).GetItemAtPosition(e.Position));

        var foodList = dataManager_.dataRef_.Where(elm => elm.date_.Date == selectedDate);
        intent_ = new Android.Content.Intent(this, typeof(CalendarCell.CalendarCellActivity));
        intent_.PutExtra("date", selectedDate.ToString("yyyy年MM月dd日"));
        intent_.PutParcelableArrayListExtra("card", foodList.ToArray());
        if(checkPermission(false)) StartActivityForResult(intent_, 0x00);
        else requestPermission(false);
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


    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults) {
      Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

      base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

      switch(requestCode) {
      case 0xFF:
        if(grantResults.Where(p => p == Permission.Denied).Count() == 0 && intent_ != null) StartActivityForResult(intent_, 0x00);
        break;
      }
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


    // Runtime Permission check
    private bool checkPermission(bool _cold) {
      // 既に許可している
      return ((CheckSelfPermission(Android.Manifest.Permission.Camera) == Permission.Granted &
        CheckSelfPermission(Android.Manifest.Permission.WriteExternalStorage) == Permission.Granted) |
        !_cold) &&
        CheckSelfPermission(Android.Manifest.Permission.ReadExternalStorage) == Permission.Granted;
    }

    // 許可を求める
    private void requestPermission(bool _cold) {
      string[] req;
      if(_cold) {
        req = new[] {
          Android.Manifest.Permission.Camera,
          Android.Manifest.Permission.WriteExternalStorage,
          Android.Manifest.Permission.ReadExternalStorage };
      } else {
        req = new[] {
          Android.Manifest.Permission.ReadExternalStorage };
      }

      if(ShouldShowRequestPermissionRationale(Android.Manifest.Permission.Camera) ||
        ShouldShowRequestPermissionRationale(Android.Manifest.Permission.WriteExternalStorage) ||
        ShouldShowRequestPermissionRationale(Android.Manifest.Permission.ReadExternalStorage)) {
        RequestPermissions(req, 0xFF);
      } else {
        RequestPermissions(req, 0xFF);
      }
    }
  }
}

