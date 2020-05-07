using Autofac;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;
using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace MediatorEventBroker
{
    public class Actor
    {
        protected EventBroker _broker;

        public Actor(EventBroker broker)
        {
            _broker = broker;
        }
    }

    public class FootballPlayer : Actor, IDisposable
    {
        private CompositeDisposable _subscription = new CompositeDisposable();

        public int GoalsScored { get; private set; }
        public string Name { get; }

        public FootballPlayer(EventBroker broker, string name) : base(broker)
        {
            Name = name;

            _subscription.Add(
                broker.OfType<PlayerScoredEvent>()
                    .Where(ps => ps.Name != name)
                    .Subscribe(
                    ev =>
                    {
                        if (ev.GoalsScored < 3)
                        {
                            Debug.WriteLine($"{name}: nicely done, {ev.Name}! It's your {ev.GoalsScored} goal.");
                        }
                    }
                ));

            _subscription.Add(
                broker.OfType<PlayerSentOffEvent>()
                    .Where(ps => ps.Name != name)
                    .Subscribe(
                        ev => Debug.WriteLine($"{name}: see you in the lockers, {ev.Name}.")
                    )
                );
        }

        public void Score()
        {
            GoalsScored++;

            _broker.Publish(new PlayerScoredEvent { GoalsScored = GoalsScored, Name = Name });
        }

        public void AssaultReferee()
        {
            _broker.Publish(new PlayerSentOffEvent { Name = Name, Reason = "violence" });
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }
    }

    public class FootballCoach : Actor, IDisposable
    {
        private CompositeDisposable _subscription = new CompositeDisposable();

        public FootballCoach(EventBroker broker) : base(broker)
        {
            _subscription.Add(
                broker.OfType<PlayerScoredEvent>().Subscribe(
                    ev =>
                    {
                        if (ev.GoalsScored < 3)
                        {
                            Debug.WriteLine($"Coach: well done, {ev.Name}!");
                        }
                    }
                ));

            _subscription.Add(
                broker.OfType<PlayerSentOffEvent>().Subscribe(
                    ev =>
                    {
                        if (ev.Reason == "violence")
                        {
                            Debug.WriteLine($"Coach: how could you, {ev.Name}?");
                        }
                    }
                ));
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }
    }

    public class PlayerEvent
    {
        public string Name { get; set; }
    }

    public class PlayerScoredEvent : PlayerEvent
    {
        public int GoalsScored { get; set; }
    }

    public class PlayerSentOffEvent : PlayerEvent
    {
        public string Reason { get; set; }
    }

    public class EventBroker : IObservable<PlayerEvent>
    {
        Subject<PlayerEvent> _subscriptions = new Subject<PlayerEvent>();

        public IDisposable Subscribe(IObserver<PlayerEvent> observer)
        {
            return _subscriptions.Subscribe(observer);
        }

        public void Publish(PlayerEvent ev)
        {
            _subscriptions.OnNext(ev);
        }
    }

    public class EventBrokerTests
    {
        [Test]
        public void Test1()
        {
            var cb = new ContainerBuilder();
            cb.RegisterType<EventBroker>().SingleInstance();
            cb.RegisterType<FootballCoach>();
            cb.Register((c, p) => 
                new FootballPlayer(
                    c.Resolve<EventBroker>(),
                    p.Named<string>("name")
                )
            );

            using (var c = cb.Build())
            {
                var coach = c.Resolve<FootballCoach>();
                var player1 = c.Resolve<FootballPlayer>(new NamedParameter("name", "John"));
                var player2 = c.Resolve<FootballPlayer>(new NamedParameter("name", "Kyle"));

                bool receivedSomething = false;

                c.Resolve<EventBroker>().Subscribe(ev =>
                {
                    receivedSomething = true;
                });

                player1.Score();
                player1.Score();
                player1.Score();
                player1.AssaultReferee();
                player2.Score();

                Assert.True(receivedSomething);
            }
        }
    }
}