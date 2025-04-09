using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text;
using System.Linq;
using QRCoder;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FrameWork
{
    public static class QrCodeGenerator
    {
        public static bool Generate(string str,
            string level = "M",
            string text = "hello world",
            string imagePath = "",
            int size = 150)
        {
            return RenderQrCode(str,
                level,
                text,
                imagePath,
                size);
        }

        public static bool RenderQrCode(string str,
            string level = "M",
            string text = "hello world",
            string imagePath = "",
            int size = 150)
        {
            try
            {
                QRCoder.QRCodeGenerator.ECCLevel eccLevel = (QRCoder.QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
                using (QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator())
                {
                    using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(str, eccLevel))
                    {
                        using (QRCode qrCode = new QRCode(qrCodeData))
                        {
                            var bmp = qrCode.GetGraphic(31,
                                Color.Black,
                                Color.White,
                                null,
                                100,
                                6,
                                true);

                            bmp.Save(imagePath + ".bmp");

                            var qualityEncoder = System.Drawing.Imaging.Encoder.Quality;
                            var quality = (long)100;
                            var ratio = new EncoderParameter(qualityEncoder, quality);
                            var codecParams = new EncoderParameters(1);
                            codecParams.Param[0] = ratio;
                            var jpegCodecInfo = ImageCodecInfo.GetImageEncoders().ToList().Where(i => i.MimeType.Equals("image/jpeg")).FirstOrDefault();

                            bmp.Save(imagePath + ".jpg",
                                jpegCodecInfo, codecParams);

                            var resizeImage = ResizeImage((Image)bmp, size, size);

                            resizeImage.Save(imagePath + "_resized.jpg");

                            return true;
                        }
                    }
                }
            }
            catch (Exception exc)
            { }
            return false;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
