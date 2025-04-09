using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
//using System.Web.UI.WebControls;
//using System.Web.SessionState;

namespace FrameWork
{
    public static class CaptchaImage
    {
        /// <summary>
        /// use: <img id="CapArea" alt="Captcha" src="@Url.Action("Action", "Controller")" style="height: 61px; margin-top: 8px; border-radius: 4px;" />
        /// </summary>
        /// <returns></returns>

        //public static Bitmap/*Image*//*FileContentResult*//*Image*/ GetCaptcha()
        //{
        //    try
        //    {
        //        var rand = new Random((int)DateTime.Now.Ticks);

        //        //generate new question
        //        int a = rand.Next(10, 99);
        //        int b = rand.Next(0, 9);
        //        long num = new Random().Next(22222, 99999);
        //        string str = num.ToString().Replace('0', '9').Replace('1', '3');
        //        string captcha = Web.FrameWork.PublicClasses.PersianType.ToFarsiDigits(str);
        //        HttpContext.Current.Session["Captcha"] = str;
        //        Bitmap img = null;

        //        using (var mem = new MemoryStream())
        //        using (var bmp = new Bitmap(380, 59))

        //        using (var gfx = Graphics.FromImage((Image)bmp))
        //        {
        //            gfx.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
        //            gfx.SmoothingMode = SmoothingMode.AntiAlias;
        //            gfx.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 400, 60));

        //            int i = 1, r, x = 20, y = 0;
        //            foreach (var item in captcha.ToArray())
        //            {
        //                gfx.DrawString(item.ToString(), new Font("Tahoma", rand.Next(20, 30), FontStyle.Strikeout), Brushes.White, x += 50, y += 5);
        //            }
        //            if (true)
        //            {
        //                var pen = new Pen(Color.White);
        //                for (i = 100; i < 390; i++)
        //                {
        //                    r = rand.Next(0, i);
        //                    x = rand.Next(0, i);
        //                    y = rand.Next(0, 390);
        //                    gfx.DrawEllipse(pen, x, y, r, r);
        //                }
        //            }
        //            bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Png);
        //            img = bmp;
        //        }
        //        return img;
        //    }
        //    catch (Exception exc)
        //    {
        //        return null;
        //    }
        //}
    }
}