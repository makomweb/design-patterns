﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericValueAdapter
{
    public interface IInteger
    {
        int Value { get; }
    }

    public static class Dimensions
    {
        public class Two : IInteger
        {
            public int Value => 2;
        }

        public class Three : IInteger
        {
            public int Value => 3;
        }

    }

    public class Vector<T, D> where D : IInteger, new()
    {
        protected T[] _data;

        public Vector()
        {
            _data = new T[new D().Value];
        }

        public Vector(params T[] values)
        {
            var requiredSize = new D().Value;
            _data = new T[requiredSize];

            var providedSize = values.Length;

            for (int i = 0; i < Math.Min(requiredSize, providedSize); ++i)
            {
                _data[i] = values[i];
            }
        }

        public static Vector<T, D> Create(params T[] values)
        {
            return new Vector<T, D>(values);
        }

        public T this[int index]
        {
            get => _data[index];
            set => _data[index] = value;
        }

        public T X
        {
            get => _data[0];
            set => _data[0] = value;
        }
    }

    public class VectorOfInt<D> : Vector<int, D>
        where D: IInteger, new()
    {
        public VectorOfInt()
        {

        }

        public VectorOfInt(params int[] values) : base(values)
        {

        }

        public static VectorOfInt<D> operator +
            (VectorOfInt<D> leftHandSide, VectorOfInt<D> rightHandSide)
        {
            var result = new VectorOfInt<D>();
            var dim = new D().Value;

            for (int i = 0; i < dim; i++)
            {
                result[i] = leftHandSide[i] + rightHandSide[i];
            }

            return result;
        }
    }

    public class VectorOfFloat<D> : Vector<float, D>
        where D : IInteger, new()
    {
    }

    public class Vector2i : VectorOfInt<Dimensions.Two>
    {
        public Vector2i()
        {

        }

        public Vector2i(params int[] values) : base(values)
        {

        }
    }

    public class Vector3f : VectorOfFloat<Dimensions.Three>
    {
        public override string ToString()
        {
            return $"{string.Join(",", _data)}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var v = new Vector2i(1, 2);
            v[0] = 0;

            var vv = new Vector2i(3, 2);

            var result = v + vv;

            var u = Vector3f.Create(3.5f, 2.2f, 1);
            u.ToString(); // error!
            u = u + u; // error!
        }
    }
}
