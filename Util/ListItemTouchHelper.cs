using System;
using System.Collections.Generic;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;

namespace GoodByeMilk.Util {
  public class ListItemTouchHelper : ItemTouchHelper.SimpleCallback {
    public Action<RecyclerView.ViewHolder, int> onSwipe;
    public Func<RecyclerView, RecyclerView.ViewHolder, RecyclerView.ViewHolder, bool> onMove;
    public ListItemTouchHelper(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) {
    }

    public ListItemTouchHelper(int dragDirs = ItemTouchHelper.Up | ItemTouchHelper.Down,
      int swipeDirs = ItemTouchHelper.Left | ItemTouchHelper.Right) : base(dragDirs, swipeDirs) {

    }

    public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target) {
      if(onMove != null) return onMove(recyclerView, viewHolder, target);
      else return true;
    }

    public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction) {
      onSwipe?.Invoke(viewHolder, direction);
    }
  }
}
