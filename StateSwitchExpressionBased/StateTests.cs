using NUnit.Framework;
using System.Diagnostics;
using System.Threading;

namespace StateSwitchExpressionBased
{
    enum Chest
    {
        Open,
        Closed,
        Locked
    }

    enum Action
    {
        Open,
        Close
    }

    public class StateTests
    {
        static Chest Manipulate(Chest chest, Action action, bool haveKey) =>
            (chest, action, haveKey) switch
            {
                (Chest.Locked, Action.Open, true) => Chest.Open,
                (Chest.Closed, Action.Open, _) => Chest.Open,
                (Chest.Open, Action.Close, true) => Chest.Locked,
                (Chest.Open, Action.Close, false) => Chest.Closed,
                _ => chest,
            };

        [Test]
        public void Test1()
        {
            var chest = Chest.Locked;
            Debug.WriteLine($"Chest ist {chest}");

            chest = Manipulate(chest, Action.Open, true);
            Debug.WriteLine($"Chest ist {chest}");

            chest = Manipulate(chest, Action.Close, false);
            Debug.WriteLine($"Chest ist {chest}");

            chest = Manipulate(chest, Action.Close, false);
            Debug.WriteLine($"Chest ist {chest}");
        }
    }
}