using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Adapter
{
    public class Point
    {
        public int X, Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            //return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
            return $"[{X}, {X}]";
        }
    }

    public class Line
    {
        public Point Start, End;

        public Line()
        {

        }

        public Line(Point start, Point end)
        {
            if (start == null) throw new ArgumentNullException(paramName: nameof(start));
            if (end == null) throw new ArgumentNullException(paramName: nameof(end));

            Start = start;
            End = end;
        }

        public override string ToString()
        {
            //return $"{nameof(Start)}: {Start}, {nameof(End)}: {End}";
            return $"{Start}-{End}";
        }
    }

    public class VectorObject : Collection<Line>
    {

    }

    public class VectorRectangles : VectorObject
    {
        public VectorRectangles(int x, int y, int width, int height)
        {
            Add(new Line(new Point(x, y), new Point(x + width, y)));
            Add(new Line(new Point(x + width, y), new Point(x + width, y + height)));
            Add(new Line(new Point(x, y), new Point(x, y + height)));
            Add(new Line(new Point(x, y + height), new Point(x + width, y + height)));
        }
    }

    public class LineToPointAdapter : Collection<Point>
    {
        private static int _count;

        public LineToPointAdapter(Line line)
        {
            WriteLine($"{++_count}: Generating points for line {line}");

            int left = Math.Min(line.Start.X, line.End.X);
            int right = Math.Max(line.Start.X, line.End.X);
            int top = Math.Min(line.Start.Y, line.End.Y);
            int bottom = Math.Max(line.Start.Y, line.End.Y);
            int dx = right - left;
            int dy = line.End.Y - line.Start.Y;

            if (dx == 0)
            {
                for (int y = top; y <= bottom; ++y)
                {
                    Add(new Point(left, y));
                }
            }
            else if (dy == 0)
            {
                for (int x = left; x <= right; ++x)
                {
                    Add(new Point(x, top));
                }
            }
        }
    }

    class Program
    {
        private static readonly List<VectorObject> _objects
            = new List<VectorObject>
            {
                new VectorRectangles(1,1,10,10),
                new VectorRectangles(3,3,6,6)
            };

        public static void DrawPoint(Point p)
        {
            Write(".");
        }

        static void Main(string[] args)
        {
            foreach (var obj in _objects)
            {
                foreach (var line in obj)
                {
                    var adapter = new LineToPointAdapter(line);
                    foreach (var point in adapter)
                    {
                        DrawPoint(point);
                    }
                }
            }
        }
    }
}
