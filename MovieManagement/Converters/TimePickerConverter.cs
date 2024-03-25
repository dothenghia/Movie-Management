using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManagement.Converters
{
    public class TimePickerConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime date = (DateTime)value;
            TimeSpan? time; 
            return time = new TimeSpan(date.Hour, date.Minute, 00);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
