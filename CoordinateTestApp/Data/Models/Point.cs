using System.Security.RightsManagement;

namespace CoordinateTestApp.Data.Models
{
	public struct Point
	{
        public int Id { get; set; }

        public double X { get; set; }

		public double Y { get; set; }

		public Point(double x, double y) : this()
		{
			X = x;
			Y = y;
		}

		public Point(int id, double x, double y) : this(x, y)
		{
			Id = id;
		}

		public static bool operator ==(Point point1, Point point2)
		{
			return point1.X == point2.X && point1.Y == point2.Y;
		}

		public static bool operator !=(Point point1, Point point2)
		{
			return point1.X != point2.X || point1.Y != point2.Y;
		}

		public override bool Equals(object obj)
		{
			if (obj is Point point)
			{
				return this.X == point.X && this.Y == point.Y;
			}

			return false;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode() & X.GetHashCode() & Y.GetHashCode();
		}
	}
}
