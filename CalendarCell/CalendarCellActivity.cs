
using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Content.PM;

namespace GoodByeMilk.CalendarCell {
  [Activity(Label = "CalendarCellActivity", WindowSoftInputMode = SoftInput.AdjustPan)]
  public class CalendarCellActivity : Activity {
    const int CAMERA = 0x00;
    List<Util.BabyFood> foodList_;
    List<string> imageList_;
    RecyclerView recycler_;
    RecyclerView image_;
    CalendarCellAdapter adapter_;
    Camera.CameraAdapter cameraAdapter_;
    DateTime date_;
    string path_;
    string intentPath_;
    protected override void OnCreate(Bundle savedInstanceState) {
      base.OnCreate(savedInstanceState);
      SetContentView(Resource.Layout.calendar_cell_activity);
      foodList_ = this.Intent.GetParcelableArrayListExtra("card").Cast<Util.BabyFood>().ToList();
      FindViewById<TextView>(Resource.Id.dateTimeInCell).Text = Intent.GetStringExtra("date");



      date_ = DateTime.Parse(Intent.GetStringExtra("date"));
      imageList_ = new List<string>();

      if(!System.IO.Directory.Exists(System.IO.Path.Combine(GetExternalFilesDir(null).Path, date_.Year.ToString()))) {
        System.IO.Directory.CreateDirectory(System.IO.Path.Combine(GetExternalFilesDir(null).Path, date_.Year.ToString()));
      }

      if(!System.IO.Directory.Exists(System.IO.Path.Combine(GetExternalFilesDir(null).Path, date_.Year.ToString(), date_.Month.ToString()))) {
        System.IO.Directory.CreateDirectory(System.IO.Path.Combine(GetExternalFilesDir(null).Path, date_.Year.ToString(), date_.Month.ToString()));
      }

      if(!System.IO.Directory.Exists(System.IO.Path.Combine(GetExternalFilesDir(null).Path, date_.Year.ToString(), date_.Month.ToString(), date_.Day.ToString()))) {
        System.IO.Directory.CreateDirectory(System.IO.Path.Combine(GetExternalFilesDir(null).Path, date_.Year.ToString(), date_.Month.ToString(), date_.Day.ToString()));
      }

      path_ = System.IO.Path.Combine(GetExternalFilesDir(null).Path, date_.Year.ToString(), date_.Month.ToString(), date_.Day.ToString());
      if(System.IO.Directory.Exists(path_)) {
        imageList_ = System.IO.Directory.GetFiles(path_).ToList();
      }

      // 時間が経つとデータベースから消えているので再登録
      foreach(var img in imageList_) {
        registerDatabase(img);
      }

      cameraAdapter_ = new Camera.CameraAdapter(this, imageList_);
      var cameraManager = new LinearLayoutManager(this);
      cameraManager.Orientation = LinearLayoutManager.Horizontal;
      image_ = FindViewById<RecyclerView>(Resource.Id.imageList);
      image_.HasFixedSize = false;
      image_.SetLayoutManager(cameraManager);
      image_.SetAdapter(cameraAdapter_);









      Android.Graphics.Point point = new Android.Graphics.Point();
      this.WindowManager.DefaultDisplay.GetSize(point);

      recycler_ = FindViewById<RecyclerView>(Resource.Id.foodList);
      adapter_ = new CalendarCellAdapter(this, foodList_);
      var manager_ = new LinearLayoutManager(this);
      recycler_.HasFixedSize = false;
      recycler_.SetLayoutManager(manager_);
      recycler_.LayoutParameters.Height = (int)(point.Y * 0.66667);

      recycler_.SetAdapter(adapter_);


      #region Click FAB
      FindViewById<FloatingActionButton>(Resource.Id.EditFoodMenu).Click += (sender, e) => {
        var view = LayoutInflater.Inflate(Resource.Layout.food_list_element_edit, null);
        view.SetBackgroundColor(new Android.Graphics.Color(GetColor(Resource.Color.mintcream)));
        var popup = new PopupWindow(this);
        popup.OutsideTouchable = true;
        popup.Focusable = true;

        popup.ContentView = view;
        popup.SetBackgroundDrawable(GetDrawable(Resource.Drawable.abc_popup_background_mtrl_mult));
        popup.Width = (int)(point.X * 0.8);
        popup.Height = (int)(point.Y * 0.4);


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



      FindViewById<FloatingActionButton>(Resource.Id.AddPhoto).Click += (sender, e) => {

        if(Build.VERSION.SdkInt >= BuildVersionCodes.M) {
          if(checkPermission()) cameraIntent();
          else requestPermission();
        } else {
          cameraIntent();
        }
      };
      #endregion

      #region Click List Element
      adapter_.onClick += (position) => {
        var view = LayoutInflater.Inflate(Resource.Layout.food_list_element_edit, null);
        view.SetBackgroundColor(new Android.Graphics.Color(GetColor(Resource.Color.mintcream)));

        var popup = new PopupWindow(this);
        popup.OutsideTouchable = true;
        popup.Focusable = true;

        popup.ContentView = view;
        popup.SetBackgroundDrawable(GetDrawable(Resource.Drawable.abc_popup_background_mtrl_mult));
        popup.Width = (int)(point.X * 0.8);
        popup.Height = (int)(point.Y * 0.4);



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
      #endregion

      #region Swipe List Element
      var foodItemTouchHelperCallback = new Util.ListItemTouchHelper();
      foodItemTouchHelperCallback.onSwipe += (sender, _) => {
        var archive = new KeyValuePair<int, Util.BabyFood>(((RecyclerView.ViewHolder)sender).AdapterPosition, foodList_[((RecyclerView.ViewHolder)sender).AdapterPosition]);
        foodList_.RemoveAt(((RecyclerView.ViewHolder)sender).AdapterPosition);
        adapter_.NotifyItemRemoved(((RecyclerView.ViewHolder)sender).AdapterPosition);
        //adapter_.NotifyItemRangeChanged(archive.Key, foodList_.Count - archive.Key);

        Snackbar.Make(recycler_, "データを削除しました", Snackbar.LengthLong).SetAction("元に戻す", (v) => {
          foodList_.Insert(archive.Key, archive.Value);
          adapter_.NotifyItemInserted(archive.Key);
          //adapter_.NotifyItemRangeChanged(archive.Key + 1, foodList_.Count - archive.Key + 1);
        }).Show();
      };


      var foodItemTouchHelper = new ItemTouchHelper(foodItemTouchHelperCallback);
      foodItemTouchHelper.AttachToRecyclerView(recycler_);
      #endregion


      cameraAdapter_.onClick += (position) => {
        var view = LayoutInflater.Inflate(Resource.Layout.imageview, null);
        view.SetBackgroundColor(new Android.Graphics.Color(GetColor(Resource.Color.mintcream)));

        var popup = new PopupWindow(this);
        popup.OutsideTouchable = true;
        popup.Focusable = true;

        popup.ContentView = view;
        popup.SetBackgroundDrawable(GetDrawable(Resource.Drawable.abc_popup_background_mtrl_mult));
        popup.Width = (int)(point.X * 0.8);
        popup.Height = (int)(point.Y * 0.8);

        var imageView = view.FindViewById<ImageView>(Resource.Id.image);
        imageView.SetImageURI(Android.Net.Uri.FromFile(new Java.IO.File(imageList_[position])));


        //Android.Media.ExifInterface exif = new Android.Media.ExifInterface(imageList_[position]);
        //var orientation = int.Parse(exif.GetAttribute(Android.Media.ExifInterface.TagOrientation));
        //var imageWidth = float.Parse(exif.GetAttribute(Android.Media.ExifInterface.TagImageWidth));
        //var imageHeight = float.Parse(exif.GetAttribute(Android.Media.ExifInterface.TagImageLength));
        //var viewWidth = point.X * 0.7;
        //var viewHeight = point.X * 0.75;
        //var mat = new Android.Graphics.Matrix();

        popup.ShowAtLocation(recycler_, GravityFlags.Center, 0, 0);
      };


      var cameraItemTouchHelperCallback = new Util.ListItemTouchHelper(ItemTouchHelper.Left | ItemTouchHelper.Right, ItemTouchHelper.Up | ItemTouchHelper.Down);
      cameraItemTouchHelperCallback.onSwipe += (sender, _) => {
        var archive = new KeyValuePair<int, string>(((RecyclerView.ViewHolder)sender).AdapterPosition, imageList_[((RecyclerView.ViewHolder)sender).AdapterPosition]);
        imageList_.RemoveAt(((RecyclerView.ViewHolder)sender).AdapterPosition);
        cameraAdapter_.NotifyItemRemoved(((RecyclerView.ViewHolder)sender).AdapterPosition);
        //adapter_.NotifyItemRangeChanged(archive.Key, foodList_.Count - archive.Key);
        Util.SnackbarCallback callback = new Util.SnackbarCallback();
        callback.onDismissed += () => {
          if(archive.Value != null)
            System.IO.File.Delete(archive.Value);
        };

        var snackbar = Snackbar.Make(recycler_, "データを削除しました", Snackbar.LengthLong).SetAction("元に戻す", (v) => {
          imageList_.Insert(archive.Key, archive.Value);
          cameraAdapter_.NotifyItemInserted(archive.Key);
          archive = new KeyValuePair<int, string>(0, null);
          //adapter_.NotifyItemRangeChanged(archive.Key + 1, foodList_.Count - archive.Key + 1);
        });
        snackbar.AddCallback(callback);
        snackbar.Show();
      };


      var cameraItemTouchHelper = new ItemTouchHelper(cameraItemTouchHelperCallback);
      cameraItemTouchHelper.AttachToRecyclerView(image_);


    }




    protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data) {
      base.OnActivityResult(requestCode, resultCode, data);
      switch(requestCode) {
      case CAMERA:

        if(resultCode == Result.Ok) {
          imageList_.Add(intentPath_);
          registerDatabase(intentPath_);
          cameraAdapter_.NotifyItemInserted(imageList_.Count);
        } else {
        }

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

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults) {
      base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
      if(requestCode == 0xFF) {
        // 使用が許可された
        if(grantResults.Where(p => p == Permission.Denied).Count() == 0) cameraIntent();
      }
    }




    // Runtime Permission check
    private bool checkPermission() {
      // 既に許可している
      return CheckSelfPermission(Android.Manifest.Permission.Camera) == Permission.Granted &&
        CheckSelfPermission(Android.Manifest.Permission.WriteExternalStorage) == Permission.Granted;
    }

    private void requestPermission() {
      if(ShouldShowRequestPermissionRationale(Android.Manifest.Permission.Camera) ||
        ShouldShowRequestPermissionRationale(Android.Manifest.Permission.WriteExternalStorage)) {
        RequestPermissions(new String[] {
          Android.Manifest.Permission.Camera,
          Android.Manifest.Permission.WriteExternalStorage}, 0xFF);
      } else {
        RequestPermissions(new String[] {
          Android.Manifest.Permission.Camera,
          Android.Manifest.Permission.WriteExternalStorage}, 0xFF);
      }
    }

    private void cameraIntent() {
      // ファイル名
      intentPath_ = System.IO.Path.Combine(new[] {
        GetExternalFilesDir(null).Path,
        date_.Year.ToString(),
        date_.Month.ToString(),
        date_.Day.ToString(),
        DateTime.Now.ToString("HHmmss") + ".jpg"
    });

      var cameraUri = Android.Support.V4.Content.FileProvider.GetUriForFile(this, Application.PackageName + ".fileprovider", new Java.IO.File(intentPath_));

      Intent intent = new Intent(Android.Provider.MediaStore.ActionImageCapture);
      intent.PutExtra(Android.Provider.MediaStore.ExtraOutput, cameraUri);
      StartActivityForResult(intent, CAMERA);
    }

    private void registerDatabase(string _path) {
      ContentValues contentValues = new ContentValues();
      ContentResolver contentResolver = ContentResolver;
      contentValues.Put(Android.Provider.MediaStore.Images.Media.InterfaceConsts.MimeType, "image/jpeg");
      contentValues.Put(Android.Provider.MediaStore.MediaColumns.Data, _path);
      contentResolver.Insert(Android.Provider.MediaStore.Images.Media.ExternalContentUri, contentValues);
      var intent = new Intent(Intent.ActionMediaScannerScanFile);
      intent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(_path)));
      SendBroadcast(intent);
    }
  }
}
