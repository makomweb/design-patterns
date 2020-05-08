using NUnit.Framework;
using System.Collections.Generic;

namespace Memento
{
    public class BankAccountMemento
    {
        public int Balance { get; }

        public BankAccountMemento(int balance)
        {
            Balance = balance;
        }
    }

    public class BankAccount
    {
        public int Balance { get; private set; }
        private readonly List<BankAccountMemento> _changes = new List<BankAccountMemento>();
        private int _current;

        public BankAccount(int balance)
        {
            Balance = balance;
            _changes.Add(new BankAccountMemento(Balance));
        }

        public BankAccountMemento Deposit(int amount)
        {
            Balance += amount;
            var m = new BankAccountMemento(Balance);
            _changes.Add(m);
            ++_current;
            return m;
        }

        public BankAccountMemento Restore(BankAccountMemento memento)
        {
            if (memento != null)
            {
                Balance = memento.Balance;
                _changes.Add(memento);
                return memento;
            }

            return null;
        }

        public BankAccountMemento Undo()
        {
            if (_current > 0)
            {
                var m = _changes[--_current];
                Balance = m.Balance;
                return m;
            }

            return null;
        }

        public BankAccountMemento Redo()
        {
            if (_current + 1 < _changes.Count)
            {
                var m = _changes[++_current];
                Balance = m.Balance;
                return m;
            }

            return null;
        }

        public override string ToString()
        {
            return $"Balance: {Balance}";
        }
    }

    public class MementoTests
    {
        [Test]
        public void Test_rollback_to_memento()
        {
            var ba = new BankAccount(100);

            var m1 = ba.Deposit(50); // 150
            Assert.AreEqual(150, ba.Balance);

            var m2 = ba.Deposit(25); // 175
            Assert.AreEqual(175, ba.Balance);

            ba.Restore(m1);
            Assert.AreEqual(150, ba.Balance);

            ba.Restore(m2);
            Assert.AreEqual(175, ba.Balance);
        }

        [Test]
        public void Test_undo_redo()
        {
            var ba = new BankAccount(100);

            ba.Deposit(50);
            ba.Deposit(25);

            Assert.AreEqual(175, ba.Balance);

            ba.Undo();
            Assert.AreEqual(150, ba.Balance);

            ba.Undo();
            Assert.AreEqual(100, ba.Balance);

            ba.Redo();
            Assert.AreEqual(150, ba.Balance);
        }
    }
}