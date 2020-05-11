using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;

namespace ObserverSpecialInterfaces
{
    public class ObserverTests
    {
        // subscription becomes visible - can be disposed!

        public class Event
        {

        }

        public class FallsIllEvent : Event
        {
            public string Address;
        }

        public class Person : IObservable<Event>
        {
            private readonly HashSet<Subscription> _subscriptions
                = new HashSet<Subscription>();

            public IDisposable Subscribe(IObserver<Event> observer)
            {
                var subscription = new Subscription(this, observer);

                _subscriptions.Add(subscription);

                return subscription;
            }

            private class Subscription : IDisposable
            {
                private readonly Person _person;

                public IObserver<Event> Receiver { get; }

                public Subscription(Person person, IObserver<Event> receiver)
                {
                    _person = person;
                    Receiver = receiver;
                }

                public void Dispose()
                {
                    _person._subscriptions.Remove(this);
                }
            }

            public void CatchACold()
            {
                foreach ( var s in _subscriptions)
                {
                    s.Receiver.OnNext(new FallsIllEvent { Address = "123 London Road" });
                }
            }
        }

        public class Listener : IObserver<Event>
        {
            public void OnCompleted()
            {
                // stream is closed
            }

            public void OnError(Exception error)
            {
                // error on the stream
            }

            public void OnNext(Event value)
            {
                // new item in the event stream

                if (value is FallsIllEvent args)
                {
                    Debug.WriteLine($"A doctor is required at {args.Address}");
                }
            }
        }

        [Test]
        public void Test1()
        {
            var listener = new Listener();

            var person = new Person();

            var sub = person.Subscribe(listener);

            person.OfType<FallsIllEvent>()
                .Subscribe(args => Debug.WriteLine($"Stream 2: A doctor is required at {args.Address}"));

            person.CatchACold();
        }
    }
}