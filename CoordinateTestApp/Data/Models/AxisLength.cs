using CoordinateTestApp.Data.Converters;
using System.ComponentModel;

namespace CoordinateTestApp.Data.Models
{
    [TypeConverter(typeof(AxisLengthConverter))]
	public struct AxisLength
    {
        public double Left { get; }

        public double Top { get; }

        public double Right { get; }

        public double Bottom { get; }

        public AxisLength(double uniformLength)
        {
            Left = Top = Right = Bottom = uniformLength;
        }

        public AxisLength(double left, double top, double right, double bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public bool IsValid()
        {
            if (Left < 0.0 || Right < 0.0 || Top < 0.0 || Bottom < 0.0)
            {
                return false;
            }

            if (double.IsNaN(Left) || double.IsNaN(Right) || double.IsNaN(Top) || double.IsNaN(Bottom))
            {
                return false;
            }

            if (double.IsPositiveInfinity(Left) || double.IsPositiveInfinity(Right) || double.IsPositiveInfinity(Top) || double.IsPositiveInfinity(Bottom))
            {
                return false;
            }

            if (double.IsNegativeInfinity(Left) || double.IsNegativeInfinity(Right) || double.IsNegativeInfinity(Top) || double.IsNegativeInfinity(Bottom))
            {
                return false;
            }

            return true;
        }
    }
}
