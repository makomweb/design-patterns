using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Command
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
    }

    public class BankAccountCommand : ICommand
    {
        private BankAccount _account;
        private Action _action;
        private int _amount;
        bool _succeeded;

        public BankAccountCommand(BankAccount account, Action action, int amount)
        {
            _account = account;
            _action = action;
            _amount = amount;
        }

        public void Call()
        {
            switch (_action)
            {
                case Action.Deposit:
                    _account.Deposit(_amount);
                    _succeeded = true;
                    break;
                case Action.Withdraw:
                    _succeeded = _account.Withdraw(_amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Argument {_action} is not supported!");
            }
        }

        public void Undo()
        {
            if (!_succeeded) return;

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
    public class CommandTests
    {
        [Test]
        public void Execute_commands_on_a_bank_account()
        {
            var ba = new BankAccount();
            var commands = new List<BankAccountCommand>
            {
                new BankAccountCommand(ba, BankAccountCommand.Action.Deposit, 100),
                new BankAccountCommand(ba, BankAccountCommand.Action.Withdraw, 50)
            };

            foreach(var c in commands)
            {
                c.Call();
            }

            Debug.WriteLine(ba);
        }

        [Test]
        public void Reverse_commands_on_a_bank_account()
        {
            var ba = new BankAccount();
            var commands = new List<BankAccountCommand>
            {
                new BankAccountCommand(ba, BankAccountCommand.Action.Deposit, 100),
                new BankAccountCommand(ba, BankAccountCommand.Action.Withdraw, 1000)
            };

            foreach (var c in commands)
            {
                c.Call();
            }

            Debug.WriteLine(ba);

            foreach (var c in Enumerable.Reverse(commands))
            {
                c.Undo();
            }

            Debug.WriteLine(ba);
        }
    }
}