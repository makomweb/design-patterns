using NUnit.Framework;
using System;

namespace ObserverExercise
{
    public class Game
    {
        public event EventHandler<Rat> Entered;
        public event EventHandler<Rat> Left;

        public Rat Spawn()
        {
            var rat = new Rat(this);
            Entered?.Invoke(this, rat);
            return rat;
        }

        public void Terminate(Rat rat)
        {
            Left?.Invoke(this, rat);
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
        }

        private void OnEntered(object sender, Rat rat)
        {
            if (this != rat)
                Attack += 1;
        }

        private void OnLeft(object sender, Rat rat)
        {
            if (this != rat)
                Attack -= 1;
        }

        public void Dispose()
        {
            _game.Entered -= OnEntered;
            _game.Left -= OnLeft;
            _game.Terminate(this);
        }
    }

    public class ObserverExercise
    {
        [Test]
        public void Test1()
        {
            var g = new Game();

            using var r1 = g.Spawn();
            Assert.AreEqual(1, r1.Attack);

            {
                using var r2 = g.Spawn();
                Assert.AreEqual(2, r1.Attack);
                Assert.AreEqual(2, r2.Attack);

                using var r3 = g.Spawn();
                Assert.AreEqual(3, r1.Attack);
                Assert.AreEqual(3, r2.Attack);
                Assert.AreEqual(3, r3.Attack);
            }
        }
    }
}