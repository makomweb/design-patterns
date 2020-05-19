using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventSourcing
{
    // CQRS = command query responsibility segregation
    // CQS = command query separation
    // Command = do/change

    public class Person
    {
        private int _age;
        readonly EventBroker _broker;

        public Person(EventBroker broker)
        {
            _broker = broker;
        }
    }

    public class EventBroker
    {
        // 1. all events happend
        public IList<Event> AllEvents = new List<Event>();

        // 2. commands 
        public event EventHandler<Command> Commands;

        // 3. query
        public event EventHandler<Query> Queries;
    }

    public class Query
    {

    }

    public class Command
    {

    }

    public class Event
    {
        // backtrack
    }

    public class Tests
    {
        [Test]
        public void Test1()
        {
            var p = new Person();

            Assert.NotNull(p);
        }
    }
}