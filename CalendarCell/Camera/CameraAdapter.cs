using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Provider;
using Android.Graphics;

namespace GoodByeMilk.CalendarCell.Camera {
  public class CameraAdapter : RecyclerView.Adapter {
    IReadOnlyList<string> image_;
    Context context_;
    RecyclerView recycler_;

    public Action<int> onClick;
    public CameraAdapter(Context _context, IReadOnlyList<string> _path) {
      context_ = _context;
      image_ = _path;
    }

    public CameraAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) {
    }

    public override int ItemCount => image_.Count;

    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
      ContentResolver cr = context_.ContentResolver;
      /** 画像をファイルパスから検索 */


      BitmapFactory.Options options = new BitmapFactory.Options();
      options.InJustDecodeBounds = true;
      var bitmap = BitmapFactory.DecodeFile(image_[position]);
      int imageHeight = bitmap.Height;
      int imageWidth = bitmap.Width;
      String imageType = options.OutMimeType;
      int inSampleSize = 1;

      if(imageHeight > 320 || imageWidth > 240) {
        int halfHeight = imageHeight / 2;
        int halfWidth = imageWidth / 2;

        // Calculate the largest inSampleSize value that is a power of 2 and keeps both
        // height and width larger than the requested height and width.
        while((halfHeight / inSampleSize) >= 320
                && (halfWidth / inSampleSize) >= 240) {
          inSampleSize *= 2;
        }
      }
      options.InSampleSize = inSampleSize;
      options.InJustDecodeBounds = false;
      Android.Media.ExifInterface exif = new Android.Media.ExifInterface(image_[position]);
      //var orientation = int.Parse(exif.GetAttribute(Android.Media.ExifInterface.TagOrientation));
      var thumbnail = BitmapFactory.DecodeFile(image_[position], options);
      Android.Graphics.Matrix mat = new Android.Graphics.Matrix();


      if(thumbnail.Width > thumbnail.Height) {
        mat.SetRotate(90, thumbnail.Width / 2, thumbnail.Height / 2);
        mat.PreScale(320.0f / thumbnail.Width, 240.0f / thumbnail.Height);
      } else {
        mat.PreScale(240.0f / thumbnail.Width, 320.0f / thumbnail.Height);
      }
      thumbnail = Android.Graphics.Bitmap.CreateBitmap(thumbnail, 0, 0, thumbnail.Width, thumbnail.Height, mat, true);
      ((CameraViewHolder)holder).image_.SetImageBitmap(thumbnail);

      holder.ItemView.Click -= ItemView_Click;
      holder.ItemView.Click += ItemView_Click;
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

      return new CameraViewHolder(LayoutInflater
      .From(parent.Context)
      .Inflate(Resource.Layout.imageview,
          parent,
          false));
    }
  }
}

