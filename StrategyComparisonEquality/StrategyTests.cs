using NUnit.Framework;
using System.Collections.Generic;

namespace StrategyComparisonEquality
{
    public class Person
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
    }

    public class StrategyTests
    {
        [Test]
        public void Test1()
        {
            var people = new List<Person>();

            people.Sort();
        }
    }
}