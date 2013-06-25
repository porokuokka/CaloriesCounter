using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Xaml.Data;

namespace CaloriesCounter
{
    [AttributeUsageAttribute(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class ValueConversionAttribute : Attribute
    {
        public ValueConversionAttribute(Type source, Type convert) { }

    }

    [ValueConversion(typeof(String), typeof(float))]
    public class StringToFloatConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, string str)
        {
            // value is the data from the source object.

            string input = value.ToString();
            float f;
            input.Replace(",", ".");
            float.TryParse(input, out f);
            return f.ToString("n2");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string str)
        {
            string input = value.ToString();
            float f;
            input.Replace(",", ".");
            float.TryParse(input, out f);
            return f;
        }

        #endregion
    }
}
