using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;

namespace ProxyProperty
{
    public class Property<T> : IEquatable<Property<T>> where T: new()
    {
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (Equals(_value, value)) return;
                Debug.WriteLine($"Assigning value {value}.");
                _value = value;
            }
        }
        public Property() : this(Activator.CreateInstance<T>()) // this(default(T)) 
        {

        }

        public Property(T value)
        {
            _value = value;
        }

        public static implicit operator T(Property<T> property)
        {
            return property._value; // int n = p_int;
        }

        public static implicit operator Property<T>(T value)
        {
            return new Property<T>(value); // Property<int> p = 123; 
        }

        public static bool operator ==(Property<T> left, Property<T> right)
        {
            return EqualityComparer<Property<T>>.Default.Equals(left, right);
        }

        public static bool operator !=(Property<T> left, Property<T> right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return obj is Property<T> property &&
                   EqualityComparer<T>.Default.Equals(_value, property._value) &&
                   EqualityComparer<T>.Default.Equals(Value, property.Value);
        }

        public bool Equals(Property<T> other)
        {
            return other != null &&
                   EqualityComparer<T>.Default.Equals(_value, other._value) &&
                   EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }

    public class Creature
    {
        private Property<int> _agility = new Property<int>();

        public int Agility
        {
            get => _agility.Value;
            set => _agility.Value = value;
        }
    }

    public class PropertyProxyTests
    {
        [Test]
        public void Test1()
        {
            var c = new Creature();
            c.Agility = 10; // c.set_Agility(10) ! is not whats supposed to be!
                            // c.Agility = new Property<int>(10); 

            Assert.AreEqual(10, c.Agility);

            c.Agility = 10;
            Assert.AreEqual(10, c.Agility);
        }
    }
}