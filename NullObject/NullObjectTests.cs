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

        public BankAccount(ILog log)
        {
            if (log == null) throw new ArgumentNullException(paramName: nameof(log));
            _log = log;
        }

        public void Deposit(int amount)
        {
            Balance += amount;
            _log.Info($"Deposited {amount}, balance is now {Balance}");
        }
    }

    public class NullObjectTests
    {
        [Test]
        public void Test1()
        {
            var log = new ConsoleLog();
            var ba = new BankAccount(log);
            ba.Deposit(100);
            Assert.True(ba.Balance != 0);
        }
    }
}