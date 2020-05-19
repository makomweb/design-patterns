using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace EventSourcing
{
    // CQRS = command query responsibility segregation
    // CQS = command query separation
    // Command = do/change

    public class Person : IDisposable
    {
        readonly EventBroker _broker;

        public int Age { get; private set; }

        public Person(EventBroker broker, int age)
        {
            _broker = broker;
            _broker.Commands += OnBrokerCommands;
            _broker.Queries += OnBrokerQueries;

            Age = age;
        }

        private void OnBrokerQueries(object sender, Query e)
        {
            if (e is AgeQuery query && query.Target == this)
            {
                e.Result = Age;
            }
        }

        private void OnBrokerCommands(object sender, Command e)
        {
            if (e is ChangeAgeCommand command && command.Target == this)
            {
                _broker.AddEvent(new AgeChangedEvent(this, Age, command.NewAge));
                Age = command.NewAge;
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

        public void UndoLast()
        {
            var e = AllEvents.LastOrDefault();
            if (e is AgeChangedEvent ace)
            {
                Command(new ChangeAgeCommand(ace.Target, ace.OldValue));
                AllEvents.Remove(e);
            }
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

        public override string ToString()
        {
            return $"Person {Target} changed age from {OldValue} to {NewValue}.";
        }
    }

    public class Tests
    {
        [Test]
        public void Test1()
        {
            var eb = new EventBroker();
            var p = new Person(eb) { Age = 32 };

            eb.Command(new ChangeAgeCommand(p, 33));

            foreach(var e in eb.AllEvents)
            {
                Debug.WriteLine(e);
            }

            var age = eb.Query<int>(new AgeQuery(p));

            Assert.AreEqual(33, age);


        }
    }
}