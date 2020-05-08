using Autofac;
using ImpromptuInterface;
using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Dynamic;

namespace NullObject
{
    public interface ILog
    {
        void Info(string msg);
        void Warn(string msg);
    }

    public class ConsoleLog : ILog
    {
        public void Info(string msg)
        {
            Debug.WriteLine(msg);
        }

        public void Warn(string msg)
        {
            Debug.WriteLine("Warning!!! " + msg);
        }
    }

    public class NullLog : ILog
    {
        public void Info(string msg)
        {
        }

        public void Warn(string msg)
        {
        }
    }

    public class BankAccount
    {
        private readonly ILog _log;
        public int Balance { get; private set; }

        public BankAccount([CanBeNull] ILog log)
        {
            _log = log;
        }

        public void Deposit(int amount)
        {
            Balance += amount;
            _log?.Info($"Deposited {amount}, balance is now {Balance}");
        }
    }

    public class Null<TInterface> : DynamicObject where TInterface : class
    {
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            result = Activator.CreateInstance(binder.ReturnType);
            return true;
        }
    }

    public class NullObjectTests
    {
        [Test]
        public void Test1()
        {
            //var log = new ConsoleLog();
            var ba = new BankAccount(/*log*/ null);
            ba.Deposit(100);
            Assert.True(ba.Balance != 0);
        }

        [Test]
        public void Test_using_a_container()
        {
            var cb = new ContainerBuilder();
            cb.RegisterType<BankAccount>();
            cb.RegisterType<NullLog>()
                .As<ILog>();
            using (var c = cb.Build())
            {
                var ba = c.Resolve<BankAccount>();
                Assert.NotNull(ba);
            }
        }

        [Test]
        public void Different_approach_for_null_object()
        {
            var log = new Null<ILog>().ActLike<ILog>();
            log.Info("blablabla");
            var ba = new BankAccount(log);
            ba.Deposit(1000);
        }
    }
}