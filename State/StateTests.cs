using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace State
{
    public enum State
    {
        OffHook,
        Connecting,
        Connected,
        OnHold
    }

    public enum Trigger
    {
        CallDialed,
        HungUp,
        CallConnected,
        PlacedOnHold,
        TakenOffHold,
        LeftMessage
    }


    public class StateTests
    {
        private static Dictionary<State, List<(Trigger, State)>> _rules
            = new Dictionary<State, List<(Trigger, State)>>
            {
                [State.OffHook] = new List<(Trigger, State)>
                {
                    (Trigger.CallDialed, State.Connecting)
                },
                [State.Connecting] = new List<(Trigger, State)>
                {
                    (Trigger.HungUp, State.OffHook),
                    (Trigger.CallConnected, State.Connected),
                },
                [State.OffHook] = new List<(Trigger, State)>
                {
                    (Trigger.LeftMessage, State.OffHook),
                    (Trigger.HungUp, State.OffHook),
                    (Trigger.PlacedOnHold, State.OnHold)
                },
                [State.OffHook] = new List<(Trigger, State)>
                {
                    (Trigger.TakenOffHold, State.Connected),
                    (Trigger.HungUp, State.OffHook)
                },
            };

        [Test]
        public void Test1()
        {
            var state = State.OffHook;

            while (true)
            {
                Debug.WriteLine($"The phone is currently {state}");
                Debug.WriteLine("Select a trigger:");

                for (int i = 0; i < _rules[state].Count; i++)
                {
                    var (t, _) = _rules[state][i];

                    Debug.WriteLine($"{i}. {t}");
                }

                int input = int.Parse(Console.ReadLine());

                var (_, s) = _rules[state][input];

                state = s;
            }
        }
    }
}