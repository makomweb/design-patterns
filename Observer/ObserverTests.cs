using NUnit.Framework;
using System;

namespace Observer
{
    public class Person
    {
        public event EventHandler<EventArgs> FallsIll;

        public void CatchACold()
        {
            FallsIll?.Invoke(this, EventArgs.Empty);
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

        private void CallDoctor(object sender, EventArgs e)
        {
            _received = true;
        }
    }
}