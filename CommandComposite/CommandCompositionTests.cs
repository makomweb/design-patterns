using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CommandComposite
{
    public class BankAccount
    {
        private int _balance;

        private int _overdraftlimit = -500;

        public void Deposit(int amount)
        {
            _balance += amount;
            Debug.WriteLine($"Deposited ${amount}, balance is now {_balance}");
        }

        public bool Withdraw(int amount)
        {
            if (_balance - amount >= _overdraftlimit)
            {
                _balance -= amount;
                Debug.WriteLine($"Withdrew ${amount}, balance is now {_balance}");
                return true;
            }
            else
            {
                Debug.WriteLine($"Cannot withdraw ${amount}, balance is now {_balance}");
                return false;
            }
        }

        public override string ToString()
        {
            return $"Balance: {_balance}";
        }
    }

    public interface ICommand
    {
        void Call();

        void Undo();

        bool Succeeded { get; set; }
    }

    public class BankAccountCommand : ICommand
    {
        private BankAccount _account;
        private Action _action;
        private int _amount;

        public BankAccountCommand(BankAccount account, Action action, int amount)
        {
            _account = account;
            _action = action;
            _amount = amount;
        }

        public bool Succeeded { get; set; }

        public void Call()
        {
            switch (_action)
            {
                case Action.Deposit:
                    _account.Deposit(_amount);
                    Succeeded = true;
                    break;
                case Action.Withdraw:
                    Succeeded = _account.Withdraw(_amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Argument {_action} is not supported!");
            }
        }

        public void Undo()
        {
            if (!Succeeded) return;

            switch (_action)
            {
                case Action.Deposit:
                    _account.Withdraw(_amount);
                    break;
                case Action.Withdraw:
                    _account.Deposit(_amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Argument {_action} is not supported!");
            }
        }

        public enum Action { Deposit, Withdraw }
    }

    public class CompositeBankAccountCommand : List<BankAccountCommand>, ICommand
    {
        public CompositeBankAccountCommand()
        {

        }

        public CompositeBankAccountCommand(IEnumerable<BankAccountCommand> commands)
            : base(commands)
        {

        }

        public virtual void Call()
        {
            ForEach(cmd => cmd.Call());
        }

        private IEnumerable<BankAccountCommand> This() => this;

        public virtual void Undo()
        {
            foreach (var c in This().Reverse())
            {
                if (c.Succeeded) c.Undo();
            }
        }

        public virtual bool Succeeded
        {
            get
            {
                return this.All(cmd => cmd.Succeeded);
            }
            set
            {
                foreach (var cmd in this)
                {
                    cmd.Succeeded = value;
                }
            }
        }

        public class MoneyTransferCommand : CompositeBankAccountCommand
        {
            public MoneyTransferCommand(BankAccount from, BankAccount to, int amount)
            {
                AddRange(new[]
                    {
                        new BankAccountCommand(from,
                            BankAccountCommand.Action.Withdraw, amount),
                        new BankAccountCommand(to,
                            BankAccountCommand.Action.Deposit, amount)
                    }
                );
            }

            public override void Call()
            {
                BankAccountCommand last = null;
                foreach (var cmd in this)
                {
                    if (last == null || last.Succeeded)
                    {
                        cmd.Call();
                        last = cmd;
                    }
                    else
                    {
                        cmd.Undo();
                        break;
                    }
                }
            }
        }

        public class CommandCompositionTests
        {
            [Test]
            public void Test1()
            {
                var ba = new BankAccount();

                var deposit = new BankAccountCommand(ba,
                    BankAccountCommand.Action.Deposit, 100);

                var withdraw = new BankAccountCommand(ba,
                    BankAccountCommand.Action.Withdraw, 50);

                var composite = new CompositeBankAccountCommand(
                    new[] { withdraw, deposit });

                composite.Call();

                Debug.WriteLine(ba);

                composite.Undo();
                Debug.WriteLine(ba);
            }

            [Test]
            public void Test_money_transfer()
            {
                var from = new BankAccount();
                from.Deposit(100);
                var to = new BankAccount();

                Debug.WriteLine($"From account: {from}");
                Debug.WriteLine($"To account: {to}");

                var transfer = new MoneyTransferCommand(from, to, 100);

                transfer.Call();
                Debug.WriteLine($"From account: {from}");
                Debug.WriteLine($"To account: {to}");

                transfer.Undo();
                Debug.WriteLine($"From account: {from}");
                Debug.WriteLine($"To account: {to}");

                Assert.True(transfer.Succeeded);
            }
        }
    }
}