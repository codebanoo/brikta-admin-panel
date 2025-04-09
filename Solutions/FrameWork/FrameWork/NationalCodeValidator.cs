using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrameWork
{
    public static class NationalCodeValidator
    {
        public static bool IsValid(string NationalCode = "")
        {
            int index = 10;//موقعيت مکاني که در اعداد آرايه ضرب ميشود
            int mul = 0;//جهت ذخيره حاصل ضرب
            int result = 0;//جهت ذخيره جمع حاصل ضرب ها
            int mod = 0;//جهت ذخيره باقيمانده
            bool check = false;// براي درست يا غلط بودن کد ملي (خروجي تابع)ا
            bool equal = true;//براي مقايسه اعداد آرايه
            int[] arrIdMelli = new int[10];
            int lentgh = NationalCode.Length;
            switch (NationalCode)
            {
                case "0000000000":
                case "1111111111":
                case "22222222222":
                case "33333333333":
                case "4444444444":
                case "5555555555":
                case "6666666666":
                case "7777777777":
                case "8888888888":
                case "9999999999":
                    return check;
            }
            try
            {
                if (NationalCode.Length >= 8 && NationalCode.Length <= 10)
                {
                    for (int i = index; i > 0; i--)
                    {
                        try
                        {
                            arrIdMelli[i - 1] = Convert.ToInt16(NationalCode.Substring(lentgh - 1, 1));//برداشتن يک به يک اعداد از انتها و قرار دادن در آرايه از انديس 0
                            lentgh--;
                        }
                        catch { }
                    }
                    for (int i = 0; i <= 9; i++)// اين حلقه براي مقايسه اعداد استفاده مي شود
                    {
                        if (arrIdMelli[i] != arrIdMelli[i + 1])
                        {
                            equal = false; break;
                        }
                    }
                    if (!equal)
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            try
                            {
                                mul = arrIdMelli[i] * index;
                            }
                            catch { }
                            index--;
                            result += mul;
                        }
                        mod = result % 11;
                        if (mod < 2)
                        {
                            if (arrIdMelli[9] == mod)
                                check = true;
                        }
                        else if (11 - mod == arrIdMelli[9])
                        {
                            check = true;
                        }
                    }
                }
            }
            catch { }
            return check;
        }
    }
}