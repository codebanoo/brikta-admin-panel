using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace FrameWork
{
    public class PersianDate
    {
        private static PersianCalendar pc;
        private static PersianCalendar calendar
        {
            get
            {
                if (pc == null)
                    pc = new PersianCalendar();
                return pc;
            }
        }

        public static string DateNow
        {
            get
            {
                string result = "{0:D4}/{1:D2}/{2:D2}";
                result = string.Format(result, ThisYear, ThisMonth, Today);
                return result;
            }
        }

        public static string DateNowMonthDesc
        {
            get
            {
                string result = "{0:D4} {1} {2:D2}";
                result = string.Format(result, ThisYear, ThisMonthString, Today);
                return result;
            }
        }

        public static string DateNowFullDesc
        {
            get
            {
                string result = "{3} {2:D2} {1} {0:D4}";
                result = string.Format(result, ThisYear, ThisMonthString, Today, TodayOfWeek);
                return result;
            }
        }

        public static string TimeNow
        {
            get
            {
                DateTime now = DateTime.Now;
                string result = "{0:D2}:{1:D2}:{2:D2}";
                result = string.Format(result, calendar.GetHour(now), calendar.GetMinute(now), calendar.GetSecond(now));
                return result;
            }
        }

        public static int ThisYear
        {
            get
            {
                return calendar.GetYear(DateTime.Now);
            }
        }

        public static int ThisMonth
        {
            get
            {
                return calendar.GetMonth(DateTime.Now);
            }
        }

        public static string ThisMonthString
        {
            get
            {
                string[] months = { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
                return months[calendar.GetMonth(DateTime.Now) - 1];
            }
        }

        public static int Today
        {
            get
            {
                return calendar.GetDayOfMonth(DateTime.Now);
            }
        }

        public static string TodayOfWeek
        {
            get
            {
                string result = "";
                switch (calendar.GetDayOfWeek(DateTime.Now))
                {
                    case System.DayOfWeek.Saturday:
                        result = "شنبه";
                        break;
                    case System.DayOfWeek.Sunday:
                        result = "یکشنبه";
                        break;
                    case System.DayOfWeek.Monday:
                        result = "دوشنبه";
                        break;
                    case System.DayOfWeek.Tuesday:
                        result = "سه شنبه";
                        break;
                    case System.DayOfWeek.Wednesday:
                        result = "چهارشنبه";
                        break;
                    case System.DayOfWeek.Thursday:
                        result = "پنجشنبه";
                        break;
                    case System.DayOfWeek.Friday:
                        result = "جمعه";
                        break;
                }
                return result;
            }
        }

        public static string DateFirstYear
        {
            get
            {
                string result = "{0:D4}/01/01";
                result = string.Format(result, ThisYear);
                return result;
            }
        }

        public static string DateEndYear
        {
            get
            {
                string result = "{0:D4}/12/{1:D2}";
                result = string.Format(result, ThisYear, calendar.IsLeapYear(ThisYear) ? 30 : 29);
                return result;
            }
        }

        public static string PersianDateFrom(DateTime dt)
        {
            string result = "{0:D4}/{1:D2}/{2:D2}";
            result = string.Format(result, calendar.GetYear(dt), calendar.GetMonth(dt), calendar.GetDayOfMonth(dt));
            return result;
        }

        public static DateTime ToGregorianDate(string persianDate)
        {
            DateTime result = DateTime.MinValue;
            string[] items = persianDate.Split('/');
            if (items.Length == 3)
            {
                int year = Convert.ToInt32(items[0]);
                int month = Convert.ToInt32(items[1]);
                int day = Convert.ToInt32(items[2]);
                result = calendar.ToDateTime(year, month, day, 0, 0, 0, 0);
            }
            return result;
        }

        public static DateTime ToGregorianDate(string persianDate, string time)
        {
            DateTime result = ToGregorianDate(persianDate);
            if (result != DateTime.MinValue)
            {
                string[] items = time.Split(':');
                int hour = 0;
                int min = 0;
                int sec = 0;
                if (items.Length == 3)
                {
                    hour = Convert.ToInt32(items[0]);
                    min = Convert.ToInt32(items[1]);
                    sec = Convert.ToInt32(items[2]);
                }
                else if (items.Length == 2)
                {
                    hour = Convert.ToInt32(items[0]);
                    min = Convert.ToInt32(items[1]);
                }
                result.Add(new TimeSpan(hour, min, sec));
            }
            return result;
        }

        public static TimeSpan DifferenceDateTime(string startPersianDate, string startTime, string endPersianDate, string endTime)
        {
            //TimeSpan result = new TimeSpan();
            DateTime fromDate = ToGregorianDate(startPersianDate, startTime);
            DateTime toDate = ToGregorianDate(endPersianDate, endTime);
            TimeSpan result = toDate.Subtract(fromDate);
            return result;
        }
    }
}