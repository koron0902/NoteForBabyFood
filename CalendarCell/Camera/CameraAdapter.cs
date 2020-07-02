using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Provider;

namespace GoodByeMilk.CalendarCell.Camera {
  public class CameraAdapter : RecyclerView.Adapter {
    IReadOnlyList<string> image_;
    Context context_;
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
      var cursor = cr.Query(
                          MediaStore.Images.Media.ExternalContentUri,
                          null,
                          MediaStore.Images.ImageColumns.Data + " = ?",
                          new String[] { image_[position] },
                          null);

      if(cursor != null && cursor.MoveToFirst()) {
        /** 画像のIDを取得 */
        long id = cursor.GetLong(cursor.GetColumnIndex(MediaStore.Images.ImageColumns.Id));
        Android.Media.ExifInterface exif = new Android.Media.ExifInterface(image_[position]);
        //var orientation = int.Parse(exif.GetAttribute(Android.Media.ExifInterface.TagOrientation));
        /** IDから96x96のサムネイルを取得 */

        var thumbnail = MediaStore.Images.Thumbnails.GetThumbnail(
                cr, id, ThumbnailKind.MiniKind, null);
        cursor.Close();
        Android.Graphics.Matrix mat = new Android.Graphics.Matrix();


        if(thumbnail.Width > thumbnail.Height) {
          mat.SetRotate(90, thumbnail.Width / 2, thumbnail.Height / 2);
          mat.PreScale(320.0f / thumbnail.Width, 240.0f / thumbnail.Height);
        } else {
          mat.PreScale(240.0f / thumbnail.Width, 320.0f / thumbnail.Height);
        }
        thumbnail = Android.Graphics.Bitmap.CreateBitmap(thumbnail, 0, 0, thumbnail.Width, thumbnail.Height, mat, true);
        ((CameraViewHolder)holder).image_.SetImageBitmap(thumbnail);


      }
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

