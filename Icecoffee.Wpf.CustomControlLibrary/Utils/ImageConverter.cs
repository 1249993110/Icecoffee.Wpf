using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IceCoffee.Wpf.CustomControlLibrary.Utils
{
    public static class ImageConverter
    {
        public static Bitmap ConvertImageSourceToBitmap(ImageSource imageSource, 
            System.Drawing.Imaging.PixelFormat pixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppPArgb)
        {
            BitmapSource m = imageSource as BitmapSource;
            Bitmap bmp = new Bitmap(m.PixelWidth, m.PixelHeight, pixelFormat);

            var bitmapData = bmp.LockBits(
                new Rectangle(System.Drawing.Point.Empty, bmp.Size),
                System.Drawing.Imaging.ImageLockMode.WriteOnly, pixelFormat);

            m.CopyPixels(Int32Rect.Empty, bitmapData.Scan0, bitmapData.Height * bitmapData.Stride, bitmapData.Stride);

            bmp.UnlockBits(bitmapData);

            return bmp;
        }


        public static BitmapSource ConvertBitmapToImageSource(Bitmap bitmap)
        {
            return ConvertBitmapToImageSource(bitmap, PixelFormats.Bgra32);
        }

        public static BitmapSource ConvertBitmapToImageSource(Bitmap bitmap, PixelFormat pixelFormat)
        {
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                pixelFormat, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);

            return bitmapSource;
        }
    }
}
