using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace BuilderAdvanced
{
    class Person
    {
        public string StreetAddress, PostCode, City;
        public string CompanyName, Position;
        public int AnualIncome;

        public override string ToString()
        {
            return $"{nameof(StreetAddress)}: {StreetAddress}, " +
                   $"{nameof(PostCode)}: {PostCode}, " +
                   $"{nameof(City)}: {City}, " +
                   $"{nameof(CompanyName)}: {CompanyName}, " +
                   $"{nameof(Position)}: {Position}, " +
                   $"{nameof(AnualIncome)}: {AnualIncome}";
        }
    }

    class PersonBuilder // fascade
    {
        // reference type!
        protected Person _person = new Person();

        public PersonJobBuilder Works => new PersonJobBuilder(_person);

        public PersonAddressBuilder Lives => new PersonAddressBuilder(_person);

        public Person Build()
        {
            return _person;
        }
    }

    class PersonJobBuilder : PersonBuilder
    {
        public PersonJobBuilder(Person person)
        {
            _person = person;
        }

        public PersonJobBuilder At(string company)
        {
            _person.CompanyName = company;
            return this;
        }

        public PersonJobBuilder AsA(string position)
        {
            _person.Position = position;
            return this;
        }

        public PersonJobBuilder Earning(int amount)
        {
            _person.AnualIncome = amount;
            return this;
        }
    }

    class PersonAddressBuilder : PersonBuilder
    {
        public PersonAddressBuilder(Person person)
        {
            _person = person;
        }

        public PersonAddressBuilder At(string streetAddress)
        {
            _person.StreetAddress = streetAddress;
            return this;
        }

        public PersonAddressBuilder In(string city, string postCode)
        {
            _person.City = city;
            _person.PostCode = postCode;
            return this;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var pb = new PersonBuilder();

            var person = pb
                .Works.At("Fabrikam")
                .At("Engineer")
                .Earning(50000)
                .Lives.In("London", "22345")
                .Build();


            WriteLine(person);
        }
    }
}
