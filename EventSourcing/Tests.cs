using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace EventSourcing
{
    // CQRS = command query responsibility segregation
    // CQS = command query separation
    // Command = do/change

    public class Person : IDisposable
    {
        private int _age;
        readonly EventBroker _broker;

        public Person(EventBroker broker)
        {
            _broker = broker;
            _broker.Commands += OnBrokerCommands;
            _broker.Queries += OnBrokerQueries;
        }

        private void OnBrokerQueries(object sender, Query e)
        {
            if (e is AgeQuery query && query.Target == this)
            {
                e.Result = _age;
            }
        }

        private void OnBrokerCommands(object sender, Command e)
        {
            if (e is ChangeAgeCommand command && command.Target == this)
            {
                _broker.AddEvent(new AgeChangedEvent(this, _age, command.NewAge));
                _age = command.NewAge;
            }
        }

        public void Dispose()
        {
            _broker.Commands -= OnBrokerCommands;
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

        public void Command(Command c)
        {
            Commands?.Invoke(this, c);
        }

        public T Query<T>(Query q)
        {
            Queries?.Invoke(this, q);
            return (T)q.Result;
        }

        public void AddEvent(Event ev)
        {
            AllEvents.Add(ev);
        }
    }

    public class Query
    {
        public object Result;
    }

    public class AgeQuery : Query
    {
        public Person Target;

        public AgeQuery(Person target)
        {
            Target = target;
        }
    }

    public class Command : EventArgs
    {
    }

    public class ChangeAgeCommand : Command
    {
        public Person Target;
        public int NewAge;

        public ChangeAgeCommand(Person target, int newAge)
        {
            Target = target;
            NewAge = newAge;
        }
    }

    public class Event
    {
        // backtrack
    }

    public class AgeChangedEvent : Event
    {
        public Person Target;
        public int OldValue, NewValue;

        public AgeChangedEvent(Person target, int oldValue, int newValue)
        {
            Target = target;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    public class Tests
    {
        [Test]
        public void Test1()
        {
            var eb = new EventBroker();
            var p = new Person(eb);

            eb.Command(new ChangeAgeCommand(p, 33));

            var age = eb.Query<int>(new AgeQuery(p));

            Assert.AreEqual(33, age);
        }
    }
}