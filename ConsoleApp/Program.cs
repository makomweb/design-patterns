using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns
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

    class Relationships
    {
        private readonly List<Tuple<Person, Relationship, Person>> relations =
            new List<Tuple<Person, Relationship, Person>>();

        public void AddParentChild(Person parent, Person child)
        {
            relations.Add(new Tuple<Person, Relationship, Person>(parent, Relationship.Parent, child));
            relations.Add(new Tuple<Person, Relationship, Person>(child, Relationship.Child, parent));

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
        }
    }
}
