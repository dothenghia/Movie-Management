using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManagement.Converters
{
    public class ExpiredStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Convert boolean value to string
            bool isExpired = (bool)value;
            return isExpired ? "Expired" : "Available";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // Convert string back to boolean value
            string status = value as string;
            return (status == "Expired");
        }
    }
}
