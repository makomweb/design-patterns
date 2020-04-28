using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;

namespace CommandExercise
{
    public class Command
    {
        public enum Action
        {
            Deposit,
            Withdraw
        }

        public Action TheAction;
        public int Amount;
        public bool Success;
    }

    public class Account
    {
        public int Balance { get; set; }

        public void Process(Command c)
        {
            switch (c.TheAction)
            {
                case Command.Action.Deposit:
                    Balance += c.Amount;
                    c.Success = true;
                    break;
                case Command.Action.Withdraw:
                    if (Balance - c.Amount >= 0)
                    {
                        Balance -= c.Amount;
                        c.Success = true;
                    }
                    else
                    {
                        c.Success = false;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unsupported action type {c.TheAction}!");
            }
        }
    }

    public class CommandExercise
    {
        [Test]
        public void Test_processing_account()
        {
            var ba = new Account { Balance = 55 };

            var c = new Command { TheAction = Command.Action.Deposit, Amount = 100 };

            ba.Process(c);

            Assert.AreEqual(155, ba.Balance);
            Assert.True(c.Success);
        }
    }
}