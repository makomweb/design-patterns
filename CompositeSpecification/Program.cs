using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeSpecification
{
    public class Product
    {

    }

    public class Color
    {

    }

    public abstract class ISpecification<T>
    {
        public abstract bool IsSatisfied(T p);

        public static ISpecification<T> operator &(
            ISpecification<T> first, ISpecification<T> second)
        {
            return new AndSpecification<T>(first, second);
        }
    }

    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        protected readonly ISpecification<T>[] _specs;

        public CompositeSpecification(params ISpecification<T>[] specs)
        {
            _specs = specs;
        }
    }

    // combinator
    internal class AndSpecification<T> : CompositeSpecification<T>
    {
        public AndSpecification(params ISpecification<T>[] specs) : base(specs)
        {
        }

        public override bool IsSatisfied(T p)
        {
            return _specs.All(o => o.IsSatisfied(p));
        }
    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    public class ColorSpecification : ISpecification<Product>
    {
        private Color _color;

        public ColorSpecification(Color color)
        {
            _color = color;
        }

        public override bool IsSatisfied(Product p)
        {
            return p.Color == _color;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
