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
    static string databaseName_ => "babyFood.db";
    static string tableName_ => "food";
    static int databaseVersion_ = 1;
    static BabyFood food = new BabyFood();


    public DataManager(Context context) : base(context, System.IO.Path.Combine(context.GetExternalFilesDir(null).Path, databaseName_), null, databaseVersion_) {
      data_ = new List<BabyFood>();
      dataRef_ = data_;
      data_.AddRange(Select());
    }


    public void AddData(List<BabyFood> _new) {
      data_.AddRange(_new);
      Insert(_new);
    }

    public void RemoveData(List<BabyFood> _remove) {
      data_.RemoveAll(elm => _remove.Where(mle => elm.hash_ == mle.hash_).Count() != 0);
      Remove(_remove.Select(elm => elm.hash_));
    }

    public void RemoveData(BabyFood _remove) {
      data_.RemoveAll(elm => elm.hash_ == _remove.hash_);
      Remove(new[] { _remove.hash_ });
    }

    public override void OnCreate(SQLiteDatabase db) {
      //throw new NotImplementedException();
      var query = "create table  " + tableName_ + "(" +
        nameof(food.hash_).Trim('_').ToUpper() + " text," +
        nameof(food.date_).Trim('_').ToUpper() + " text," +
        nameof(food.kind_).Trim('_').ToUpper() + " integer," +
        nameof(food.menu_).Trim('_').ToUpper() + " text," +
        nameof(food.quantity_).Trim('_').ToUpper() + " integer," +
        nameof(food.unit_).Trim('_').ToUpper() + " text);";
      db.ExecSQL(query);
    }

    public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
      db.ExecSQL("drop table if exists;" + tableName_);
      OnCreate(db);
    }

    public override void OnOpen(SQLiteDatabase db) {
      base.OnOpen(db);
    }

    public override void OnConfigure(SQLiteDatabase db) {
      base.OnConfigure(db);
    }

    void Remove(IEnumerable<string> _hash) {
      var db = this.WritableDatabase;
      foreach(var h in _hash) {
        db.Delete(tableName_, nameof(food.hash_).Trim('_').ToUpper() + " == ?", new string[] { h });
      }
    }

    void Insert(IEnumerable<BabyFood> _food) {
      var db = this.WritableDatabase;
      foreach(var f in _food) {
        var value = new ContentValues();
        value.Put(nameof(f.hash_).Trim('_').ToUpper(), f.hash_);
        value.Put(nameof(f.date_).Trim('_').ToUpper(), f.date_.ToString("yyyy-MM-dd"));
        value.Put(nameof(f.kind_).Trim('_').ToUpper(), (int)f.kind_);
        value.Put(nameof(f.menu_).Trim('_').ToUpper(), f.menu_);
        value.Put(nameof(f.quantity_).Trim('_').ToUpper(), f.quantity_);
        value.Put(nameof(f.unit_).Trim('_').ToUpper(), f.unit_);
        db.Insert(tableName_, null, value);
      }

    }

    void Update(IEnumerable<BabyFood> _food) {
      var db = this.WritableDatabase;

      foreach(var f in _food) {
        var value = new ContentValues();
        value.Put(nameof(f.hash_).Trim('_').ToUpper(), f.hash_);
        value.Put(nameof(f.date_).Trim('_').ToUpper(), f.date_.ToString("yyyy-MM-dd"));
        value.Put(nameof(f.kind_).Trim('_').ToUpper(), (int)f.kind_);
        value.Put(nameof(f.menu_).Trim('_').ToUpper(), f.menu_);
        value.Put(nameof(f.quantity_).Trim('_').ToUpper(), f.quantity_);
        value.Put(nameof(f.unit_).Trim('_').ToUpper(), f.unit_);
        db.Update(tableName_, value, nameof(f.hash_).Trim('_').ToUpper() + " == ?", new string[] { f.hash_ });
      }
    }

    IEnumerable<BabyFood> Select() {
      var db = this.ReadableDatabase;

      var cursor = db.Query(tableName_,
        new string[] {

        nameof(food.hash_).Trim('_').ToUpper(),
        nameof(food.date_).Trim('_').ToUpper(),
        nameof(food.kind_).Trim('_').ToUpper(),
        nameof(food.menu_).Trim('_').ToUpper(),
        nameof(food.quantity_).Trim('_').ToUpper(),
        nameof(food.unit_).Trim('_').ToUpper()},
        null,
        null,
        null,
        null,
        null);


      List<BabyFood> foods = new List<BabyFood>();
      cursor.MoveToFirst();
      for(var i = 0;i < cursor.Count;i++) {
        foods.Add(new BabyFood((BabyFood.Kind)cursor.GetInt(2), DateTime.Parse(cursor.GetString(1)), cursor.GetString(3), cursor.GetString(5), cursor.GetInt(4), cursor.GetString(0)));
        cursor.MoveToNext();
      }
      cursor.Close();

      return foods;
    }

    IEnumerable<BabyFood> Select(string _from, string _until) {
      var db = this.ReadableDatabase;

      return new List<BabyFood>();
    }

    IEnumerable<BabyFood> Select(BabyFood.Kind _kind) {
      var db = this.ReadableDatabase;

      return new List<BabyFood>();
    }

    IEnumerable<BabyFood> Select(string _menu) {
      var db = this.ReadableDatabase;

      return new List<BabyFood>();
    }
  }
}
