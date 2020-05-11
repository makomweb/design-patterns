using NUnit.Framework;
using System;

namespace ObserverExercise
{
    public class Game
    {
        public event EventHandler Entered;
        public event EventHandler Left;
        public event EventHandler<Rat> UpdateRequested;

        public void NotifyEntered(Rat rat)
        {
            Entered?.Invoke(rat, EventArgs.Empty);
        }

        public void NotifyUpdateRequested(Rat sender, Rat other)
        {
            UpdateRequested?.Invoke(sender, other);
        }

        public void NotifyLeft(Rat rat)
        {
            Left?.Invoke(rat, EventArgs.Empty);
        }
    }

    public class Rat : IDisposable
    {
        public int Attack = 1;
        private readonly Game _game;

        public Rat(Game game)
        {
            _game = game;
            _game.Entered += OnEntered;
            _game.Left += OnLeft;
            _game.UpdateRequested += OnUpdateRequested;
            _game.NotifyEntered(this);
        }

        private void OnUpdateRequested(object sender, Rat rat)
        {
            if (this == rat)
            {
                Attack++;
            }
        }

        private void OnLeft(object sender, EventArgs e)
        {
            Attack--;
        }

        private void OnEntered(object sender, EventArgs e)
        {
            if (this != sender)
            {
                Attack++;
                _game.NotifyUpdateRequested(this, (Rat)sender);
            }
        }

        public void Dispose()
        {
            _game.NotifyLeft(this);
        }
    }

    public class ObserverExercise
    {
        [Test]
        public void Test_rules()
        {
            Assert.That(typeof(Game).GetFields(), Is.Empty);
            Assert.That(typeof(Game).GetProperties(), Is.Empty);
        }

        [Test]
        public void Test_1_rat()
        {
            var g = new Game();
            var r = new Rat(g);
            Assert.That(r.Attack, Is.EqualTo(1));
        }

        [Test]
        public void Test_2_rats()
        {
            var g = new Game();
            var r1 = new Rat(g);
            var r2 = new Rat(g);
            Assert.That(r1.Attack, Is.EqualTo(2));
            Assert.That(r2.Attack, Is.EqualTo(2));
        }

        [Test]
        public void Test_3_rats_1_dies()
        {
            var g = new Game();

            using var r1 = new Rat(g);
            Assert.AreEqual(1, r1.Attack);

            using var r2 = new Rat(g);
            Assert.AreEqual(2, r1.Attack);
            Assert.AreEqual(2, r2.Attack);

            {
                using var r3 = new Rat(g);
                Assert.AreEqual(3, r1.Attack);
                Assert.AreEqual(3, r2.Attack);
                Assert.AreEqual(3, r3.Attack);
            }

            Assert.AreEqual(2, r1.Attack);
            Assert.AreEqual(2, r2.Attack);
        }
    }
}