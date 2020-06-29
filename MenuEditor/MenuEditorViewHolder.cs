using System;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace GoodByeMilk.MenuEditor {
  public class MenuEditorViewHolder :RecyclerView.ViewHolder{
    public Spinner when_;
    public EditText what_;
    public EditText quant_;
    public EditText unit_;
    public MenuEditorViewHolder(View _itemView) : base(_itemView) {
      what_ = _itemView.FindViewById<EditText>(Resource.Id.editorFoodName);
      quant_ = _itemView.FindViewById<EditText>(Resource.Id.editorQuant);
      unit_ = _itemView.FindViewById<EditText>(Resource.Id.editorUnit);


    }
  }
}
