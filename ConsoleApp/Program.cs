using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns
{
    class Rectangle
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }
    }
    class Program
    {
        static int Area(Rectangle r) => r.Width * r.Height;

        static void Main(string[] args)
        {
            var rec = new Rectangle(2, 3);

            Console.WriteLine($"{rec} has area: {Area(rec)}");
        }
    }
}
