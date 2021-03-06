﻿using System;
using System.Collections.Generic;

namespace SOLID_OCP
{
    enum Color
    {
        Red, Green, Blue
    }

    enum Size
    {
        Small, Medium, Large, Yuge
    }

    class Product
    {
        public string Name;
        public Color Color;
        public Size Size;

        public Product(string name, Color color, Size size)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(paramName: nameof(name));
            }

            Name = name;
            Color = color;
            Size = size;
        }
    }

    class ProductFilter
    {
        public IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (var p in products)
            {
                if (p.Size == size)
                {
                    yield return p;
                }
            }
        }

        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
        {
            foreach (var p in products)
            {
                if (p.Color == color)
                {
                    yield return p;
                }
            }
        }

        public IEnumerable<Product> FilterBySizeAndColor(IEnumerable<Product> products, Size size, Color color)
        {
            foreach (var p in products)
            {
                if (p.Size == size && p.Color == color)
                {
                    yield return p;
                }
            }
        }
    }

    interface ISpecification<T>
    {
        bool IsSatisfied(T item);
    }

    interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> specification);
    }

    class ColorSpecification : ISpecification<Product>
    {
        private Color _color;

        public ColorSpecification(Color color)
        {
            _color = color;
        }

        public bool IsSatisfied(Product item)
        {
            return item.Color == _color;
        }
    }

    class SizeSpecification : ISpecification<Product>
    {
        private readonly Size _size;

        public SizeSpecification(Size size)
        {
            _size = size;
        }
        public bool IsSatisfied(Product item)
        {
            return item.Size == _size;
        }
    }

    class AndSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _one;
        private readonly ISpecification<T> _other;

        public AndSpecification(ISpecification<T> one, ISpecification<T> other)
        {
            _one = one;
            _other = other;
        }

        public bool IsSatisfied(T item)
        {
            return _one.IsSatisfied(item) && _other.IsSatisfied(item);
        }
    }

    class AdvancedProductFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> specification)
        {
            foreach (var item in items)
            {
                if (specification.IsSatisfied(item))
                {
                    yield return item;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var apple = new Product("Apple", Color.Green, Size.Small);
            var tree = new Product("Tree", Color.Green, Size.Large);
            var house = new Product("House", Color.Blue, Size.Large);

            Product[] products = { apple, tree, house };
            var pf = new ProductFilter();

            Console.WriteLine("Green productss (old): ");
            foreach (var p in pf.FilterByColor(products, Color.Green))
            {
                Console.WriteLine($" - {p.Name} is green");
            }

            var apf = new AdvancedProductFilter();
            Console.WriteLine("Green productss (new): ");
            foreach (var p in apf.Filter(products, new ColorSpecification(Color.Green)))
            {
                Console.WriteLine($" - {p.Name} is green");
            }

            Console.WriteLine("Large blue products: ");
            foreach (var p in apf.Filter(products, new AndSpecification<Product>(new SizeSpecification(Size.Large), new ColorSpecification(Color.Blue))))
            {
                Console.WriteLine($" - {p.Name} is large and blue");
            }
        }
    }
}
