using NUnit.Framework;

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

        public BankAccount(int balance)
        {
            Balance = balance;
        }

        public BankAccountMemento Deposit(int amount)
        {
            Balance += amount;
            return new BankAccountMemento(Balance);
        }

        public void Restore(BankAccountMemento memento)
        {
            Balance = memento.Balance;
        }

        public override string ToString()
        {
            return $"Balance: {Balance}";
        }
    }

    public class MementoTests
    {
        [Test]
        public void Test1()
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
    }
}