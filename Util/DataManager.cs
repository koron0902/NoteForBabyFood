using System;
using System.Collections.Generic;
using System.Linq;
using Android.Database;
using Android.Database.Sqlite;
using Android.Content;

namespace GoodByeMilk.Util {
  public class DataManager : SQLiteOpenHelper {
    List<BabyFood> data_;
    public IReadOnlyList<BabyFood> dataRef_;
    static string databaseName_ => "babyFod.db";
    static int databaseVersion_ = 1;


    public DataManager(Context context) : base(context, System.IO.Path.Combine(context.GetExternalFilesDir(null).Path, databaseName_), null, databaseVersion_) {
      data_ = new List<BabyFood>();
      dataRef_ = data_;
      data_.Add(new BabyFood(BabyFood.Kind.EVENING, DateTime.Today, "aa", "aa", 1));
      data_.Add(new BabyFood(BabyFood.Kind.EVENING, DateTime.Today, "aa", "aa", 1));
      data_.Add(new BabyFood(BabyFood.Kind.EVENING, DateTime.Today, "aa", "aa", 1));
      data_.Add(new BabyFood(BabyFood.Kind.EVENING, DateTime.Today, "aa", "aa", 1));
      data_.AddRange()
    }


    public void AddData(List<BabyFood> _new) {
      data_.AddRange(_new);
    }

    public void RemoveData(List<BabyFood> _remove) {
      data_.RemoveAll(elm => _remove.Where(mle => elm.hash_ == mle.hash_).Count() != 0);
    }

    public void RemoveData(BabyFood _remove) {
      data_.RemoveAll(elm => elm.hash_ == _remove.hash_);
    }

    public override void OnCreate(SQLiteDatabase db) {
      throw new NotImplementedException();
    }

    public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
      throw new NotImplementedException();
    }

    void Remove() {

    }

    void Insert() {

    }

    void Update() {

    }

    IEnumerable<BabyFood> Select() {
      return new List<BabyFood>();
    }
  }
}
