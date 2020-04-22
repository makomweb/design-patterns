using NUnit.Framework;
using System;

namespace StaticDecoratorComposition
{
    public abstract class Shape
    {
        public abstract string AsString();
    }

    public class Circle : Shape
    {
        private float _radius;

        public Circle() : this(0.0f)
        {

        }

        public Circle(float radius)
        {
            _radius = radius;
        }

        public void Resize(float factor)
        {
            _radius *= factor;
        }

        override public string AsString()
        {
            return $"A circle with radius {_radius}";
        }
    }

    public class Square : Shape
    {
        private float _side;

        public Square() : this(0.0f)
        {

        }

        public Square(float side)
        {
            _side = side;
        }

        public void Resize(float factor)
        {
            _side *= factor;
        }

        override public string AsString()
        {
            return $"A square with side {_side}";
        }
    }

    public class ColoredShape : Shape
    {
        private Shape _shape;
        private string _color;

        public ColoredShape(Shape shape, string color)
        {
            if (string.IsNullOrEmpty(color)) throw new ArgumentNullException(paramName: nameof(color));
            _color = color;
            _shape = shape ?? throw new ArgumentNullException(paramName: nameof(shape));
        }

        override public string AsString()
        {
            return $"{_shape.AsString()} with color {_color}";
        }
    }

    public class TransparentShape : Shape
    {
        private Shape _shape;
        private float _transparency;

        public TransparentShape(Shape shape, float transparency)
        {
            _transparency = transparency;
            _shape = shape ?? throw new ArgumentNullException(paramName: nameof(shape));
        }

        override public string AsString()
        {
            return $"{_shape.AsString()} with {_transparency * 100.0}% transparency";
        }
    }

    public class ColoredShape<T> : Shape where T : Shape, new()
    {
        private string _color;
        private T _shape = new T();

        public ColoredShape() : this("black")
        {

        }

        public ColoredShape(string color)
        {
            if (string.IsNullOrEmpty(color)) throw new ArgumentNullException(paramName: nameof(color));
            _color = color;
        }

        public override string AsString()
        {
            return $"{_shape.AsString()} with color {_color}";
        }
    }

    public class TransparentShape<T> : Shape where T : Shape, new()
    {
        private float _transparency;
        private T _shape = new T();

        public TransparentShape() : this(0.0f)
        {

        }

        public TransparentShape(float transparency)
        {
            _transparency = transparency;
        }

        public override string AsString()
        {
            return $"{_shape.AsString()} with {_transparency * 100}% transparency";
        }
    }

    public class StaticDecoratorComposition
    {
        [Test]
        public void RunGenericTest()
        {
            var redSquare = new ColoredShape<Square>("red");
            var s = redSquare.AsString();
            Assert.False(string.IsNullOrEmpty(s));
        }

        [Test]
        public void RunTransparencyTest()
        {
            var circle = new TransparentShape<ColoredShape<Circle>>(0.4f);
            var s = circle.AsString();
            Assert.False(string.IsNullOrEmpty(s));
        }

        [Test]
        public void RunTest()
        {
            var s = new Square(1.23f);
            {
                var result = s.AsString();
                Assert.False(string.IsNullOrEmpty(result));
            }

            var redSquare = new ColoredShape(s, "red");
            {
                var result = redSquare.AsString();
                Assert.False(string.IsNullOrEmpty(result));
            }

            var transparentSquare = new TransparentShape(s, 0.5f);
            {
                var result = transparentSquare.AsString();
                Assert.False(string.IsNullOrEmpty(result));
            }

            var redTransparentSquare = new TransparentShape(redSquare, 0.5f);
            {
                var result = redTransparentSquare.AsString();
                Assert.False(string.IsNullOrEmpty(result));
            }
        }
    }
}