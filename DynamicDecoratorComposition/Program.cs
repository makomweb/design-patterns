using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace DynamicDecoratorComposition
{
    public interface IShape
    {
        string AsString();
    }

    public class Circle : IShape
    {
        private float _radius;

        public Circle(float radius)
        {
            _radius = radius;
        }

        public void Resize(float factor)
        {
            _radius *= factor;
        }

        public string AsString()
        {
            return $"A circle with radius {_radius}";
        }
    }

    public class Square : IShape
    {
        private float _side;

        public Square(float side)
        {
            _side = side;
        }

        public void Resize(float factor)
        {
            _side *= factor;
        }

        public string AsString()
        {
            return $"A square with side {_side}";
        }
    }

    public class ColoredShape : IShape
    {
        private IShape _shape;
        private string _color;

        public ColoredShape(IShape shape, string color)
        {
            if (string.IsNullOrEmpty(color)) throw new ArgumentNullException(paramName: nameof(color));
            _color = color;
            _shape = shape ?? throw new ArgumentNullException(paramName: nameof(shape));            
        }

        public string AsString()
        {
            return $"{_shape.AsString()} with color {_color}";
        }
    }

    public class TransparentShape : IShape
    {
        private IShape _shape;
        private float _transparency;

        public TransparentShape(IShape shape, float transparency)
        {
            _transparency = transparency;
            _shape = shape ?? throw new ArgumentNullException(paramName: nameof(shape));
        }

        public string AsString()
        {
            return $"{_shape.AsString()} with {_transparency * 100.0}% transparency";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var s = new Square(1.23f);
            WriteLine(s.AsString());

            var redSquare = new ColoredShape(s, "red");
            WriteLine(redSquare.AsString());

            var transparentSquare = new TransparentShape(s, 0.5f);
            WriteLine(transparentSquare.AsString());

            var redTransparentSquare = new TransparentShape(redSquare, 0.5f);
            WriteLine(redTransparentSquare.AsString());
        }
    }
}
