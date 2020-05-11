using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace ObserverRx
{
    public class Market
    {
        public BindingList<float> Prices = new BindingList<float>();

        public void Add(float price)
        {
            Prices.Add(price);

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
            m.Prices.ListChanged += (sender, args) =>
            {
                if (args.ListChangedType == ListChangedType.ItemAdded)
                {
                    float price = ((BindingList<float>)sender)[args.NewIndex];

                    Debug.WriteLine($"A new price was added {price}.");
                }
            };

            m.Add(123);
        }
    }
}