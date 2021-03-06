﻿using System;

namespace SOLID_LSP
{
    class Rectangle
    {
        public virtual int Width { get; set; }

        public virtual int Height { get; set; }

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

    class Square : Rectangle
    {
        public Square(int width) : base(width, width)
        {
        }

        public override int Width
        {
            set { base.Width = value; base.Height = value; }
        }

        public override int Height
        {
            set { base.Width = value; base.Height = value; }
        }
    }

    class Program
    {
        static int Area(Rectangle r) => r.Width * r.Height;

        static void Main(string[] args)
        {
            var rec = new Rectangle(2, 3);
            Console.WriteLine($"{rec} has area: {Area(rec)}");

            Rectangle sqr = new Square(4);
            sqr.Width = 5;
            Console.WriteLine($"{sqr} has area: {Area(sqr)}");
        }
    }
}
