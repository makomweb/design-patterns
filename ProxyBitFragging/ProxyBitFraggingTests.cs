using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ProxyBitFragging
{
    public enum Op : byte
    {
        //[Description("*")]
        Mul = 0,

        //[Description("/")]
        Div = 1,

        //[Description("+")]
        Add = 2,

        //[Description("-")]
        Sub = 3
    }

    // op --> name
    public static class OpImpl
    {
        private static readonly Dictionary<Op, char> _opNames =
            new Dictionary<Op, char>();

        private static readonly Dictionary<Op, Func<double, double, double>> _opImpl =
            new Dictionary<Op, Func<double, double, double>>
            {
                [Op.Mul] = ((x, y) => x * y),
                [Op.Div] = ((x, y) => x / y),
                [Op.Add] = ((x, y) => x + y),
                [Op.Sub] = ((x, y) => x - y),
            };

        public static double Call(this Op op, int x, int y)
        {
            return _opImpl[op](x, y);
        }

        public static string Name(this Op op)
        {
            switch (op)
            {
                case Op.Mul: return "*";
                case Op.Div: return "/";
                case Op.Add: return "+";
                case Op.Sub: return "-";
                default: throw new ArgumentException($"Unsupported op type: {op}");
            }
        }
    }

    public class Problem
    {
        // 1 3 5 7
        // Add Mul Add

        private readonly List<int> _numbers;
        private readonly List<Op> _ops;

        public Problem(IEnumerable<int> numbers, IEnumerable<Op> ops)
        {
            _numbers = numbers.ToList();
            _ops = ops.ToList();
        }

        public int Eval()
        {
            var opGroups = new[]
            {
                    new [] {  Op.Mul, Op.Div },
                    new [] {Op.Add, Op.Sub }
                };

        startAgain:
            foreach (var group in opGroups)
            {
                for (int idx = 0; idx < _ops.Count; ++idx)
                {
                    if (group.Contains(_ops[idx]))
                    {
                        var op = _ops[idx];
                        double result = op.Call(_numbers[idx], _numbers[idx + 1]);

                        if (result != (int)result)
                        {
                            return int.MinValue;
                        }

                        _numbers[idx] = (int)result;
                        _numbers.RemoveAt(idx + 1);
                        _ops.RemoveAt(idx);

                        if (_numbers.Count == 1) return _numbers[0];

                        goto startAgain;
                    }
                }
            }

            return _numbers[0];
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            int i = 0;

            for (; i < _ops.Count; ++i)
            {
                sb.Append(_numbers[i]);
                sb.Append(_ops[i].Name());
            }

            sb.Append(_numbers[i]);

            return sb.ToString();
        }
    }

    public class TwoBitSet
    {
        // 64 bits --> 32 values
        private readonly ulong _data;

        public TwoBitSet(ulong data)
        {
            _data = data;
        }

        public byte this[int index]
        {
            get
            {
                // 00 10 01 01
                var shift = index << 1; // doubles

                // 00 11 00 00
                var mask = (0b11U << shift);

                // 00 10 00 00 >> shift
                // 00 00 00 00 00 00 00 10 // = result
                return (byte)((_data & mask) >> shift);
            }
        }
    }
    public class ProxyBitFraggingTests
    {
        [Test]
        public void Test1()
        {
            // 01010101001111010
            // FTFTFTFTFF
            // 0/1 - 2 values - false / true

            // Example

            // 1 2 3 7
            // 0 1 2 ... 10

            // * / + - 4 different operators

            var numbers = new[] { 1, 3, 5, 7 };
            int numberOfOps = numbers.Length - 1;

            for (int result = 0; result <= 10; ++result)
            {
                for (var key = 0UL; key < (1UL << 2 * numberOfOps); ++key)
                {
                    var tbs = new TwoBitSet(key);
                    var ops = Enumerable.Range(0, numberOfOps)
                        .Select(i => tbs[i]).Cast<Op>().ToArray();

                    var problem = new Problem(numbers, ops);

                    if (problem.Eval() == result)
                    {
                        Debug.WriteLine($"{new Problem(numbers, ops)} = {result}");
                        break;
                    }
                }
            }
        }
    }
}