using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace BridgeExercise
{
    public interface IRenderer
    {
        string Render(string name);
    }

    public class VectorRenderer : IRenderer
    {
        public string Render(string name)
        {
            return $"Drawing {name} as lines";
        }
    }

    public class RasterRenderer : IRenderer
    {
        public string Render(string name)
        {
            return $"Drawing {name} as pixels";
        }
    }

    public abstract class Shape
    {
        protected readonly IRenderer _renderer;

        public string Name { get; set; }

        public Shape(IRenderer renderer)
        {
            if (renderer == null)
            {
                throw new ArgumentNullException(nameof(renderer));
            }

            _renderer = renderer;
        }

        public override string ToString()
        {
            return _renderer.Render(Name);
        }
    }

    public class Triangle : Shape
    {
        public Triangle(IRenderer renderer) : base(renderer)
        {
            Name = "Triangle";
        }
    }

    public class Square : Shape
    {
        public Square(IRenderer renderer) : base(renderer)
        {
            Name = "Square";
        }
    }

    public class VectorSquare : Square
    {
        public VectorSquare(IRenderer renderer) : base(renderer)
        {
        }
    }

    public class RasterSquare : Square
    {
        public RasterSquare(IRenderer renderer) : base(renderer)
        {
        }
    }

    public class VectorTriangle : Square
    {
        public VectorTriangle(IRenderer renderer) : base(renderer)
        {
        }
    }

    public class RasterTriangle : Square
    {
        public RasterTriangle(IRenderer renderer) : base(renderer)
        {
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var t = new Triangle(new RasterRenderer());
            WriteLine(t.ToString());
        }
    }
}
