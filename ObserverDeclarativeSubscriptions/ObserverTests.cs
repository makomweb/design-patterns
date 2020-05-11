using Autofac;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Reflection;

namespace ObserverDeclarativeSubscriptions
{
    public interface IEvent { }

    public interface ISend<T> where T : IEvent
    {
        event EventHandler<T> Emitted;
    }

    public interface IHandle<T> where T: IEvent
    {
        void Handle(object sender, T args);
    }

    public class ButtonPressedEvent : IEvent
    {
        public int NumberOfClicks;
    }

    public class Button : ISend<ButtonPressedEvent>
    {
        public event EventHandler<ButtonPressedEvent> Emitted;

        public void Fire(int clicks)
        {
            Emitted?.Invoke(this, new ButtonPressedEvent { NumberOfClicks = clicks });
        }
    }

    public class Logging : IHandle<ButtonPressedEvent>
    {
        public void Handle(object sender, ButtonPressedEvent args)
        {
            Debug.WriteLine($"Button clicked {args.NumberOfClicks} times.");
        }
    }

    public class ObserverTests
    {
        [Test]
        public void Let_container_wire_the_event_subscription()
        {
            var cb = new ContainerBuilder();
            var ass = Assembly.GetExecutingAssembly();

            cb.RegisterAssemblyTypes(ass)
                .AsClosedTypesOf(typeof(ISend<>))
                .SingleInstance();


        }
    }
}