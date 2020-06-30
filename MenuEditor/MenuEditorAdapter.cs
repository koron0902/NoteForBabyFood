using System;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Support.V7.Widget.Helper;

namespace GoodByeMilk.MenuEditor {
  public class MenuEditorAdapter:RecyclerView.Adapter {
    List<Util.BabyFood> data_;
    public MenuEditorAdapter(List<Util.BabyFood> _data) {
      data_ = _data;
    }

    public override int ItemCount => data_.Count;

    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position) {
      ((MenuEditorViewHolder)holder).what_.FocusChange += (sender, e) => {
        if(position >= ItemCount)
          return;
        if(!((Android.Widget.EditText)sender).HasFocus) {
          data_[position].menu_ = ((Android.Widget.EditText)sender).Text;
        }
      };

      ((MenuEditorViewHolder)holder).unit_.FocusChange += (sender, e) => {
        if(position >= ItemCount)
          return;
        if(!((Android.Widget.EditText)sender).HasFocus) {
          data_[position].unit_ = ((Android.Widget.EditText)sender).Text;
        }
      };

      ((MenuEditorViewHolder)holder).quant_.FocusChange += (sender, e) => {
        if(position >= ItemCount)
            return;
        if(!((Android.Widget.EditText)sender).HasFocus) {
          int input;
         if(int.TryParse(((Android.Widget.EditText)sender).Text, out input)){
            data_[position].quantity_ = input;
          }
        }
      };
    }

    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType) {
      return new MenuEditorViewHolder(LayoutInflater
      .From(parent.Context)
      .Inflate(Resource.Layout.food_list_element_edit,
          parent,
          false));
    }
  }
}
