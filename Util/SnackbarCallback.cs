using System;
using Android.Support.Design.Widget;

namespace GoodByeMilk.Util {
  public class SnackbarCallback : Snackbar.Callback {
    public Action onDismissed = null;
    public SnackbarCallback() {
    }

    public override void OnDismissed(Snackbar transientBottomBar, int e) {
      base.OnDismissed(transientBottomBar, e);
      onDismissed?.Invoke();
    }

    public override void OnShown(Snackbar sb) {
      base.OnShown(sb);
    }
  }
}
