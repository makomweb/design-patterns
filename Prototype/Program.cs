using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Prototype
{
    public class Person : ICloneable
    {
        public string[] Names;
        public Address Address;

        public Person(string[] names, Address address)
        {
            if (names == null) throw new ArgumentNullException(paramName: nameof(names));
            if (address == null) throw new ArgumentNullException(paramName: nameof(address));

            Names = names;
            Address = address;
        }

        public object Clone()
        {
            return new Person(Names.ToArray(), (Address)Address.Clone());
        }

        public override string ToString()
        {
            return $"{nameof(Names)}: {string.Join(" ", Names)}, {nameof(Address)}: {Address}";
        }
    }


    public class Address : ICloneable
    {
        public string StreetName;
        public int HouseNumber;

        public Address(string streetName, int houseNumber)
        {
            if (streetName == null) throw new ArgumentNullException(paramName: nameof(streetName));

            StreetName = streetName;
            HouseNumber = houseNumber;
        }

        public object Clone()
        {
            return new Address(StreetName, HouseNumber);
        }

        public override string ToString()
        {
            return $"{nameof(StreetName)}: {StreetName}, {nameof(HouseNumber)}: {HouseNumber}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var john = new Person(new[] { "John", "Smith" },
                new Address("London Road", 123));

            var jane = (Person)john.Clone();
            jane.Names[0] = "Jane";
            jane.Address = new Address("Park Drive", 321);

            WriteLine(john);
            WriteLine(jane);
        }
    }
}
