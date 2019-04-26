using System;
using System.Collections.Generic;
using System.Linq;

namespace SOLID_DIP
{
    enum Relationship
    {
        Parent, Child, Sibling
    }

    class Person
    {
        public string Name;
        //public DateTime DateOfBirth;
    }

    interface IRelationshipBrowser
    {
        IEnumerable<Person> FindAllChildren(string name);
    }

    class Relationships : IRelationshipBrowser
    {
        private readonly List<Tuple<Person, Relationship, Person>> relations =
            new List<Tuple<Person, Relationship, Person>>();

        public void AddParentChild(Person parent, Person child)
        {
            relations.Add(new Tuple<Person, Relationship, Person>(parent, Relationship.Parent, child));
            relations.Add(new Tuple<Person, Relationship, Person>(child, Relationship.Child, parent));
        }

        public IEnumerable<Person> FindAllChildren(string name)
        {
            foreach (var r in relations.Where(
                x => x.Item1.Name == name &&
                     x.Item2 == Relationship.Parent))
            {
                yield return r.Item3;
            }
        }
    }

    class Research
    {
        public Research(IRelationshipBrowser browser)
        {
            var children = browser.FindAllChildren("John");
            foreach (var child in children)
            {
                Console.WriteLine($"John has a child called {child.Name}.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var parent = new Person { Name = "John" };
            var child1 = new Person { Name = "Max" };
            var child2 = new Person { Name = "Mary" };

            var relationships = new Relationships();
            relationships.AddParentChild(parent, child1);
            relationships.AddParentChild(parent, child2);

            var research = new Research(relationships);
        }
    }
}

