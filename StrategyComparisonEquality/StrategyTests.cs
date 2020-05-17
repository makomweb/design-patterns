using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace StrategyComparisonEquality
{
    public class Person : IComparable<Person>, IComparable
    {
        public int Id;
        public string Name;
        public int Age;

        public Person(int id, string name, int age)
        {
            Id = id;
            Name = name;
            Age = age;
        }

        private sealed class NameRelationalComparer : IComparer<Person>
        {
            public int Compare([AllowNull] Person x, [AllowNull] Person y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
            }
        }

        public static IComparer<Person> NameComparer { get; } = new NameRelationalComparer();

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(this, obj)) return 0;
            if (ReferenceEquals(null, obj)) return 1;
            return obj is Person other ? CompareTo(other) 
                : throw new ArgumentException($"Object must be of type {nameof(Person)}!");
        }

        public int CompareTo([AllowNull] Person other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Id.CompareTo(other.Id);
        }

        public static bool operator <(Person left, Person right)
        {
            return Comparer<Person>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(Person left, Person right)
        {
            return Comparer<Person>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(Person left, Person right)
        {
            return Comparer<Person>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(Person left, Person right)
        {
            return Comparer<Person>.Default.Compare(left, right) >= 0;
        }
    }

    public class StrategyTests
    {
        private List<Person> _people = new List<Person>
            {
                new Person(23, "Peter", 44),
                new Person(11, "Paul", 33),
                new Person(665, "Mary", 30),
            };

        [Test]
        public void Sort_by_id()
        {
            Assert.AreEqual(23, _people.First().Id);

            // default strategy
            _people.Sort();

            Assert.AreEqual(11, _people.First().Id);
        }

        [Test]
        public void Sort_by_name()
        {
            Assert.AreEqual(23, _people.First().Id);

            // lambda used as strategy
            //_people.Sort((one, other) => one.Name.CompareTo(other.Name));
            _people.Sort(Person.NameComparer);

            Assert.AreEqual(665, _people.First().Id);
        }
    }
}