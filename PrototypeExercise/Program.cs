using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace PrototypeExercise
{
    public class Point
    {
        public int X, Y;

        public override string ToString()
        {
            return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
        }
    }

    public class Line
    {
        public Point Start, End;

        public Line DeepCopy()
        {
            return new Line { Start = new Point { X = Start.X, Y = Start.Y }, End = new Point { X = End.X, Y = End.Y } };
        }

        public override string ToString()
        {
            return $"{nameof(Start)}: {Start}, {nameof(End)}: {End}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var one = new Line { Start = new Point { X = 0, Y = 0 }, End = new Point { X = 10, Y = 10 } };
            var other = one.DeepCopy();

            other.Start = new Point { X = 5, Y = 4 };

            WriteLine($"line 1: {one}");
            WriteLine($"line 2: {other}");
        }
    }
}
