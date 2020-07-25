using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace IceCoffee.Wpf.CustomControlLibrary.Converters
{
    public class HexToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
             System.Globalization.CultureInfo culture)
        {
            if(value == null)
            {
                return null;
            }

            Brush brush = Brushes.Transparent;
            try
            {
                string hex = value.ToString();
                brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex.Contains(',') ? hex : "#" + hex));
            }
            catch
            {
            }
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
