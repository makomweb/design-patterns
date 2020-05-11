using Autofac;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

            // find all the ISend<>
            cb.RegisterAssemblyTypes(ass)
                .AsClosedTypesOf(typeof(ISend<>))
                .SingleInstance();

            // find all the IHandle<>
            cb.RegisterAssemblyTypes(ass)
                .Where(t => t.GetInterfaces()
                .Any(i =>
                    i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IHandle<>)
                ))
                .OnActivated(act =>
                {
                    // IHandle<Foo> -- wire with -- ISend<Foo> such as:
                    // ISend<Foo>.Emmitted += IHandle<Foo>.Handle()

                    var instanceType = act.Instance.GetType();
                    var interfaces = instanceType.GetInterfaces();
                    foreach (var i in interfaces)
                    {
                        if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandle<>))
                        {
                            // IHandle<Foo>
                            var arg0 = i.GetGenericArguments()[0];

                            // ISend<Foo> construct!
                            var senderType = typeof(ISend<>).MakeGenericType(arg0);

                            // every single ISend<Foo> in container
                            // IEnumerable<IFoo> --> every instance of IFoo
                            var allSenderTypes = typeof(IEnumerable<>)
                                .MakeGenericType(senderType);
                            // IEnumerable<ISend<Foo>>
                            var allServices = act.Context.Resolve(allSenderTypes);
                            foreach (var service in (IEnumerable)allServices)
                            {
                                var eventInfo = service.GetType().GetEvent("Emitted");
                                var handleMethod = instanceType.GetMethod("Handle");

                                var handler = Delegate.CreateDelegate(
                                    eventInfo.EventHandlerType, null, handleMethod
                                    );

                                eventInfo.AddEventHandler(service, handler);
                            }
                        }
                    }
                })
                .SingleInstance()
                .AsSelf();

            var container = cb.Build();

            var btn = container.Resolve<Button>();
            var logging = container.Resolve<Logging>();

            btn.Fire(1);
            btn.Fire(2);
        }
    }
}