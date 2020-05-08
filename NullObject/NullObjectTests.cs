using Autofac;
using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Diagnostics;

namespace NullObject
{
    public interface ILog
    {
        void Info(string msg);
        void Warn(string msg);
    }

    class ConsoleLog : ILog
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
            cb.Register(ctx => new BankAccount(null));
            using (var c = cb.Build())
            {
                var ba = c.Resolve<BankAccount>();
            }
        }
    }
}