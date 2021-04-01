using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using Color = System.Drawing.Color;

namespace IceCoffee.Wpf.CustomControlLibrary.Controls
{
    public class ColoredImage : System.Windows.Controls.Image
    {
        public static readonly DependencyProperty HexColorProperty = DependencyProperty.Register("HexColor", typeof(string), typeof(ColoredImage));

        public string HexColor
        {
            get { return (string)GetValue(HexColorProperty); }
            set { SetValue(HexColorProperty, value); }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if (string.IsNullOrEmpty(HexColor))
            {
                return;
            }

            var bitMap = Utils.ImageConverter.ConvertImageSourceToBitmap(this.Source);

            Color color = ColorTranslator.FromHtml(HexColor.Contains(',') ? HexColor : "#" + HexColor);

            for (int x = 0; x < bitMap.Width; ++x)
            {
                for (int y = 0; y < bitMap.Height; ++y)
                {
                    Color _color = bitMap.GetPixel(x, y);

                    bitMap.SetPixel(x, y, Color.FromArgb(
                        _color.A * color.A / 255,
                        _color.R * color.R / 255,
                        _color.G * color.G / 255,
                        _color.B * color.B / 255));
                }
            }
            this.Source = Utils.ImageConverter.ConvertBitmapToImageSource(bitMap);
        }
    }
}