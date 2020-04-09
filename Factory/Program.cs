using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    public enum CoordinateSystem
    {
        Cartesian,
        Polar
    }

    public class Point
    {
        private double _x, _y;

        /// <summary>
        /// Initialized a point from EITHER cartesian or polar!
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="system"></param>
        public Point(double a, double b, CoordinateSystem system = CoordinateSystem.Cartesian)
        {
            switch (system)
            {
                case CoordinateSystem.Cartesian:
                    _x = a;
                    _y = b;
                    break;

                case CoordinateSystem.Polar:
                    _x = a * Math.Cos(b);
                    _y = a * Math.Sin(b);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(system), system, null);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World!");
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }
}
