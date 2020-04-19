using System;
using System.Collections;
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

        public override bool Equals(object obj)
        {
            var point = obj as Point;
            return point != null &&
                   X == point.X &&
                   Y == point.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
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

        public override bool Equals(object obj)
        {
            var line = obj as Line;
            return line != null &&
                   EqualityComparer<Point>.Default.Equals(Start, line.Start) &&
                   EqualityComparer<Point>.Default.Equals(End, line.End);
        }

        public override int GetHashCode()
        {
            var hashCode = -1676728671;
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(Start);
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(End);
            return hashCode;
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

    public class LineToPointAdapter : IEnumerable<Point>
    {
        private static Dictionary<int, List<Point>> _cache
            = new Dictionary<int, List<Point>>();

        private static int _count;

        public LineToPointAdapter(Line line)
        {
            var hash = line.GetHashCode();
            if (_cache.ContainsKey(hash)) return;

            WriteLine($"{++_count}: Generating points for line {line}");

            var points = new List<Point>();

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
                    points.Add(new Point(left, y));
                }
            }
            else if (dy == 0)
            {
                for (int x = left; x <= right; ++x)
                {
                    points.Add(new Point(x, top));
                }
            }

            if (points.Any())
            {
                _cache.Add(hash, points);
            }
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return _cache.Values.SelectMany(x => x).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
            Draw();
            Draw();
        }

        private static void Draw()
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
