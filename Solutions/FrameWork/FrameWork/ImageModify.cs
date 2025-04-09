using System;
using System.Collections.Generic;
//using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace FrameWork
{
    public static class ImageModify
    {
        public static bool ResizeImage(string path, string fileName, int width, int height)
        {
            try
            {
                //using (Image<Rgba32> image = (Image<Rgba32>)Image.Load(path + fileName))
                using (Image image = Image.Load(path + fileName))
                {
                    image.Mutate(x => x.Resize(width, height));
                    image.Save(path + "\\" + "thumb_" + fileName);
                }
                return true;
            }
            catch (Exception exc)
            { }
            return false;
        }
        //public static Bitmap ResizeBitmap(Bitmap bitmap, int width, int height)
        //{
        //    try
        //    {
        //        Bitmap result = new Bitmap(width, height);
        //        using (Graphics graphics = Graphics.FromImage((System.Drawing.Image)result))
        //            graphics.DrawImage(bitmap, 0, 0, width, height);
        //        return result;
        //    }
        //    catch (Exception exc)
        //    {
        //        return null;
        //    }
        //}
    }
}