using FrameWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrameWork
{
    public class DateManager
    {
        public static string ConvertFromDate(string lang, DateTime? dateTime)
        {
            if (dateTime.HasValue)
                try
                {
                    switch (lang)
                    {
                        case "fa":
                            return PersianDate.PersianDateFrom(dateTime.Value);
                        case "en":
                            return dateTime.Value.ToString("yyyy/MM/dd");
                    }
                }
                catch (Exception exc)
                { }
            return "";
        }

        public static DateTime? ConvertToDate(string lang, string dateTime)
        {
            if (!string.IsNullOrEmpty(dateTime))
                try
                {
                    switch (lang)
                    {
                        case "fa":
                            return PersianDate.ToGregorianDate(dateTime.ToEnglishDigits());
                        case "en":
                            if (!string.IsNullOrEmpty(dateTime))
                                return DateTime.Parse(dateTime.ToEnglishDigits());
                            else
                                return null;
                    }
                }
                catch (Exception exc)
                { }
            return null;
        }

        public static dynamic GetCurrentDate(string lang)
        {
            switch (lang)
            {
                case "fa":
                    return PersianDate.DateNow;
                case "en":
                    return DateTime.Now.ToString("yyyy/MM/dd");
            }

            return null;
        }
    }
}
