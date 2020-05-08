using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MediatorExercise
{
    public class Participant
    {
        private readonly Mediator _mediator;

        public int Value { get; set; } = 0;

        public Participant(Mediator mediator)
        {
            _mediator = mediator;
            _mediator.Register(this);
        }

        public void Say(int n)
        {
            _mediator.Broadcast(this, n);
        }
    }

    public class Mediator
    {
        private readonly List<Participant> _participants = new List<Participant>();

        public void Register(Participant p)
        {
            if (!_participants.Any(o => o == p))
            {
                _participants.Add(p);
            }
        }

        public void Broadcast(Participant sender, int value)
        {
            foreach (var p in _participants)
            {
                if (p != sender)
                {
                    p.Value += value;
                }
            }
        }
    }

    public class MediatorExerciseTests
    {
        [Test]
        public void When_3_is_published_participants_should_increase_their_values()
        {
            var m = new Mediator();
            var p1 = new Participant(m);
            var p2 = new Participant(m) { Value = 2 };
            var p3 = new Participant(m) { Value = -10 };

            p2.Say(3);

            Assert.AreEqual(3, p1.Value);
            Assert.AreEqual(2, p2.Value);
            Assert.AreEqual(-7, p3.Value);
        }
    }
}