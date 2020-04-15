using System;

namespace Factory
{

    public class Point
    {
        private double _x, _y;

        /// <summary>
        /// Initialized a point from EITHER cartesian or polar!
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="system"></param>
        private Point(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public override string ToString()
        {
            return $"{nameof(_x)}: {_x}, {nameof(_y)}: {_y}";
        }

        public static PointFactory Factory => new PointFactory();

        public class PointFactory
        {
            public static Point NewCartesianPoint(double x, double y)
            {
                return new Point(x, y);
            }

            public static Point NewPolarPoint(double rho, double theta)
            {
                return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var point = Point.PointFactory.NewPolarPoint(1.9, Math.PI / 2);

            Console.WriteLine(point);
        }
    }
}
