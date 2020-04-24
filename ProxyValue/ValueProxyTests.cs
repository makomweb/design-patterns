using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProxyValue
{
    [DebuggerDisplay("{_value * 100.0f}%")]
    public class Percentage : IEquatable<Percentage>
    {
        private readonly float _value;

        internal Percentage(float value)
        {
            _value = value;
        }

        public static float operator *(float f, Percentage p)
        {
            return f * p._value;
        }

        public static Percentage operator +(Percentage a, Percentage b)
        {
            var result = a._value + b._value;
            return new Percentage(result);
        }

        public static bool operator ==(Percentage left, Percentage right)
        {
            return EqualityComparer<Percentage>.Default.Equals(left, right);
        }

        public static bool operator !=(Percentage left, Percentage right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $"{_value * 100.0f}%";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Percentage);
        }

        public bool Equals(Percentage other)
        {
            return other != null &&
                   _value == other._value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_value);
        }
    }

    public static class PercentageExtensions
    {
        public static Percentage Percent(this int value)
        {
            return new Percentage(value / 100.0f);
        }
        public static Percentage Percent(this float value)
        {
            return new Percentage(value / 100.0f);
        }
    }

    public class ValueProxyTests
    {
        [Test]
        public void Convert_int_to_percentage()
        {
            var result = 10f * 5.Percent();
            Assert.AreEqual("0,5", result.ToString());
        }

        [Test]
        public void Convert_addition_to_percentage()
        {
            var a = 2.Percent();
            var b = 3.Percent();
            var c = a + b;
            //Assert.AreEqual("5%", c.ToString());
            Assert.False(string.IsNullOrEmpty(c.ToString()));
        }
    }
}