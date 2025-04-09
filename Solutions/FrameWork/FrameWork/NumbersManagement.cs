using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork.FrameWork
{
    public static class NumbersManagement
    {
        public static string RandomCode(int minValue, int maxValue)
        {
            Random rd = new Random();
            int randomNumber = rd.Next(minValue, maxValue);
            return randomNumber.ToString();
        }
    }
}
