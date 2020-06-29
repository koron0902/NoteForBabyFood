using System;
//using Android.Icu.Util;
using System.Collections.Generic;
using System.Globalization;

namespace GoodByeMilk.Util {
  public class DateManager {
    public DateTime date_;
    public DateManager() {
      date_ = DateTime.Today;
      
    }

    //当月の要素を取得
    public List<DateTime> getDays() {
      //現在の状態を保持
      DateTime startDate = date_;

      //GridViewに表示するマスの合計を計算
      int count = getWeeks() * 7;

      //当月のカレンダーに表示される前月分の日数を計算
      var firstDay = new DateTime(date_.Year, date_.Month, 1);
      date_ = firstDay - new TimeSpan((int)firstDay.DayOfWeek, 0,0,0);

      List<DateTime> days = new List<DateTime>();

      for(int i = 0;i < count;i++) {
        days.Add(date_);
        date_ = date_.AddDays(1);
      }

      //状態を復元
      date_ = startDate;

      return days;
    }

    //当月かどうか確認
    public bool isCurrentMonth(DateTime date) {
      return date.Year == date_.Year && date.Month == date_.Month;
    }

    //週数を取得
    public int getWeeks() {

      Calendar myCal = CultureInfo.InvariantCulture.Calendar;
      var weeks = 0;
      var first = new DateTime(date_.Year, date_.Month, 1);
      var last = first.AddMonths(1) - new TimeSpan(1, 0, 0, 0, 0);
      var firstSun = first;
      var lastSat = last;

      while(firstSun.DayOfWeek != DayOfWeek.Sunday) {
        firstSun += TimeSpan.FromDays(1);
      }

      while(lastSat.DayOfWeek != DayOfWeek.Saturday) {
        lastSat -= TimeSpan.FromDays(1);
      }

      weeks = (lastSat.Day - firstSun.Day) / 7 + 1;
      if(firstSun > first) {
        weeks++;
      }
      if(lastSat < last) {
        weeks++;
      }

      return weeks;
    }

    //曜日を取得
    public int getDayOfWeek(DateTime date) {
      return (int)date.DayOfWeek;
    }

    //翌月へ
    public void nextMonth() {
      date_ = date_.AddMonths(1);
    }

    //前月へ
    public void prevMonth() {
      date_ = date_.AddMonths(-1);
    }
  }
}
