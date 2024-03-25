using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieManagement.Converters
{
    public class DiscountTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Convert boolean value to string
            bool isPercentageDiscount = (bool)value;
            return isPercentageDiscount ? "Percentage" : "Amount";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            // Convert string back to boolean value
            string type = value as string;
            return (type == "Percentage");
        }
    }
}
