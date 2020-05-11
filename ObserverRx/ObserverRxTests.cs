using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ObserverRx
{
    public class Market
    {
        private List<float> _prices = new List<float>();

        public void Add(float price)
        {
            _prices.Add(price);

            PriceAdded?.Invoke(this, price);
        }

        public event EventHandler<float> PriceAdded;
    }

    public class ObserverRxTests
    {
        [Test]
        public void Test1()
        {
            var m = new Market();
            m.PriceAdded += (sender, args) =>
            {
                Debug.WriteLine($"A new price was added {args}.");
            };

            m.Add(123);
        }
    }
}