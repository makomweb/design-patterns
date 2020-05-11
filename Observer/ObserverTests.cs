using NUnit.Framework;
using System;
using System.Diagnostics;

namespace Observer
{
    public class FallIllEventArgs
    {
        public string Address;
    }

    public class Person
    {
        public event EventHandler<FallIllEventArgs> FallsIll;

        public void CatchACold()
        {
            FallsIll?.Invoke(this, new FallIllEventArgs { Address = "123 London Road" });
        }
    }

    public class ObserverTests
    {
        private bool _received = false;

        [Test]
        public void Test_if_event_is_received()
        {
            var p = new Person();

            p.FallsIll += CallDoctor;

            p.CatchACold();

            Assert.True(_received);
        }

        private void CallDoctor(object sender, FallIllEventArgs e)
        {
            Debug.WriteLine($"a doctor needs to be called to {e.Address}.");
            _received = true;
        }
    }
}