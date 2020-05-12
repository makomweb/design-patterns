using NUnit.Framework;
using System.Diagnostics;

namespace StateClassic
{
    public class Switch
    {
        public State State = new OffState();

        public void On()
        {
            State.On(this);
        }

        public void Off()
        {
            State.Off(this);
        }
    }

    public abstract class State
    {
        public virtual void On(Switch sw)
        {
            Debug.WriteLine("Light is already on.");
        }

        public virtual void Off(Switch sw)
        {
            Debug.WriteLine("Light is already off.");
        }
    }

    public class OnState : State
    {
        public OnState()
        {
            Debug.WriteLine("Light turned on.");
        }

        public override void Off(Switch sw)
        {
            Debug.WriteLine("Turning light off...");
            sw.State = new OffState();
        }
    }

    public class OffState : State
    {
        public OffState()
        {
            Debug.WriteLine("Light turned off.");
        }

        public override void On(Switch sw)
        {
            Debug.WriteLine("Turning light on...");
            sw.State = new OnState();
        }
    }

    public class StateTests
    {
        [Test]
        public void Test1()
        {
            var ls = new Switch();
            ls.On();
            ls.Off();
            ls.Off();
        }
    }
}