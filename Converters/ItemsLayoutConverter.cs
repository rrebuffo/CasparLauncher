using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace CasparLauncher
{
    public class ItemsLayoutConverter : IValueConverter
    {
        public bool Rows { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int items = (int) value;
            int rows = (int) Math.Sqrt(items);
            int cols = (int) Math.Ceiling(items / (float) rows);
            if (Rows) return rows;
            else return cols;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
