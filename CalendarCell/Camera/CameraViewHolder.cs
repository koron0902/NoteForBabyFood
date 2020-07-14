using System;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace GoodByeMilk.CalendarCell.Camera {
  public class CameraViewHolder : RecyclerView.ViewHolder {
    public ImageView image_;
    public CameraViewHolder(View itemView) : base(itemView) {
      image_ = itemView.FindViewById<ImageView>(Resource.Id.image);
    }

    public CameraViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) {
    }
  }
}
