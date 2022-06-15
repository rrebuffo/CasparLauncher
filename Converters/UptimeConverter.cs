using CasparLauncher.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CasparLauncher
{
    public class UptimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StringBuilder sb = new StringBuilder();
            if(value is TimeSpan uptime)
            {
                if(uptime.TotalDays>=1)
                {
                    sb.AppendFormat(Resources.UptimeDaysFormat,uptime);
                    sb.Append(' ');
                }
                sb.AppendFormat(Resources.UptimeTimeFormat, uptime);
            }
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
