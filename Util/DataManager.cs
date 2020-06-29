using System;
using System.Collections.Generic;
using Android.OS;
using Android.Runtime;
using Java.Interop;
using System.Linq;

namespace GoodByeMilk.Util {
  public class Data : Java.Lang.Object, IParcelable {
    public enum Kind {
      MONING = 0,
      NOON,
      EVENING,
      SNACK
    };

    public static string ToString(Kind _kind) {
      switch(_kind) {
      case Kind.MONING:
        return "朝";
      case Kind.NOON:
        return "昼";
      case Kind.EVENING:
        return "夜";
      case Kind.SNACK:
        return "おやつ";
      default:
        return "";
      }
    }

    public Kind kind_;
    public DateTime date_;
    public string menu_;
    public string unit_;
    public int quantity_;
    public string hash_;
    public Data(Kind _kind, DateTime _date, string _menu, string _unit, int _quant) {
      kind_ = _kind;
      date_ = _date;
      menu_ = _menu;
      unit_ = _unit;
      quantity_ = _quant;
      var hashByte = System.Security.Cryptography.SHA512.Create().ComputeHash(((new System.Text.ASCIIEncoding()).GetBytes(kind_ + date_.ToString("yyyyMMdd") + menu_ + unit_ + quantity_)));

      var builder = new System.Text.StringBuilder();
      foreach(var b in hashByte) {
        builder.Append(b.ToString("x2"));
      }
      hash_ = builder.ToString();
    }

    public int DescribeContents() {
      return 0;
    }

    public void WriteToParcel(Parcel dest, [GeneratedEnum] ParcelableWriteFlags flags) {
      dest.WriteInt((int)kind_);
      dest.WriteString(date_.ToString("yyyy-MM-dd"));
      dest.WriteString(menu_);
      dest.WriteString(unit_);
      dest.WriteInt(quantity_);
    }

    [ExportField("CREATOR")]
    public static IParcelableCreator GetCreator() {
      return new MyParcelableCreator();
    }

    // Parcelable.Creator の代わり
    class MyParcelableCreator : Java.Lang.Object, IParcelableCreator {
      #region IParcelableCreator implementation
      Java.Lang.Object IParcelableCreator.CreateFromParcel(Parcel source) {
        var kind = (Kind)source.ReadInt();
        var date = DateTime.Parse(source.ReadString());
        var menu = source.ReadString();
        var unit = source.ReadString();
        var quantity = source.ReadInt();

        return new Data(kind, date, menu, unit, quantity);
      }

      Java.Lang.Object[] IParcelableCreator.NewArray(int size) {
        return new Java.Lang.Object[size];
      }
      #endregion
    }
  }

  public class DataManager {
    List<Data> data_;
    public IReadOnlyList<Data> dataRef_;

    public DataManager() {
      data_ = new List<Data>();
      dataRef_ = data_;
      data_.Add(new Data(Data.Kind.EVENING, DateTime.Today, "aa", "aa", 1));
      data_.Add(new Data(Data.Kind.EVENING, DateTime.Today, "aa", "aa", 1));
      data_.Add(new Data(Data.Kind.EVENING, DateTime.Today, "aa", "aa", 1));
      data_.Add(new Data(Data.Kind.EVENING, DateTime.Today, "aa", "aa", 1));
    }

    public void AddData(List<Data> _new) {
      data_.AddRange(_new);
    }

    public void RemoveData(List<Data> _remove) {
      data_.RemoveAll(elm => _remove.Where(mle => elm.hash_ == mle.hash_).Count() != 0);
    }
  }
}
