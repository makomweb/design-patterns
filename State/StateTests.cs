using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
                [State.Connected] = new List<(Trigger, State)>
                {
                    (Trigger.LeftMessage, State.OffHook),
                    (Trigger.HungUp, State.OffHook),
                    (Trigger.PlacedOnHold, State.OnHold)
                },
                [State.OnHold] = new List<(Trigger, State)>
                {
                    (Trigger.TakenOffHold, State.Connected),
                    (Trigger.HungUp, State.OffHook)
                },
            };

        [Test]
        public void Test_trigger_path()
        {
            var state = State.OffHook;

            var triggerPath = new List<Trigger>
            {
                Trigger.CallDialed,
                Trigger.CallConnected,
                Trigger.PlacedOnHold,
                Trigger.HungUp
            };

            foreach (var trigger in triggerPath)
            {
                Debug.WriteLine($"Current state: {state}");
                Debug.WriteLine("Possible triggers:");

                for (int i = 0; i < _rules[state].Count; i++)
                {
                    var (t, _) = _rules[state][i];

                    Debug.WriteLine($"{i}. {t}");
                }

                Debug.WriteLine($"Selecting trigger: {trigger}");

                var (_, s) = _rules[state].First(t => t.Item1 == trigger);                

                state = s;

                Debug.WriteLine($"New state: ------> {state}");
            }
        }
    }
}