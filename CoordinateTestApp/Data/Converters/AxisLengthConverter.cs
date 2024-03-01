using CoordinateTestApp.Data.Models;
using System.CodeDom;
using System.ComponentModel;
using System.Globalization;

namespace CoordinateTestApp.Data.Converters
{
    internal class AxisLengthConverter : TypeConverter
    {
        internal static readonly char[] separator = [',', ' '];

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || sourceType == typeof(double);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object source)
        {
            if (source != null)
            {
                if (source is string str)
                {
                    return FromString(str);
                }
                if (source is double val)
                {
                    return new AxisLength(val);
                }
            }

            throw new ArgumentException("Invalid source type");
        }


        private AxisLength FromString(string str)
        {
            string[] strValues = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            double[] doubleValues = strValues.Select(s => double.TryParse(s, out double v) ? v : 0).ToArray();
            return doubleValues.Length switch
            {
                1 => new AxisLength(doubleValues[0]),
                2 => new AxisLength(doubleValues[0], doubleValues[1], doubleValues[0], doubleValues[1]),
                4 => new AxisLength(doubleValues[0], doubleValues[1], doubleValues[2], doubleValues[3]),
                _ => throw new FormatException("InvalidAxisLength"),
            };
        }
    }
}
