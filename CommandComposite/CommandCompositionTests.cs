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
            return $"{nameof(_balance)}: {_balance} {nameof(_overdraftlimit)}: {_overdraftlimit}";
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

        public void Call()
        {
            ForEach(cmd => cmd.Call());
        }

        private IEnumerable<BankAccountCommand> This() => this;

        public void Undo()
        {
            foreach (var c in This().Reverse())
            {
                if (c.Succeeded) c.Undo();
            }
        }

        public bool Succeeded
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
        }
    }
}