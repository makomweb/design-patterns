using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ChainExercise
{
    public abstract class Creature
    {
        public int Attack { get; set; }
        public int Defense { get; set; }

        public abstract void HandleUpdate();
    }

    public class Goblin : Creature
    {
        protected readonly Game _game;

        public Goblin(Game game)
        {
            _game = game;

            HandleUpdate();
        }

        public override void HandleUpdate()
        {
            var goblinsInplay = _game.Creatures.Count;

            Defense = goblinsInplay;

            if (_game.Creatures.Any(c => c is GoblinKing))
            {
                Attack = 2;
            }
            else
            {
                Attack = 1;
            }
        }
    }

    public class GoblinKing : Goblin
    {
        public GoblinKing(Game game) : base(game)
        {
            Attack = 3;
            Defense = 3;
        }

        public override void HandleUpdate()
        {
            var goblinsInplay = _game.Creatures.Count;

            Defense = 2 + goblinsInplay;

            if (_game.Creatures.Any(c => c is GoblinKing))
            {
                Attack = 2;
            }
            else
            {
                Attack = 1;
            }
        }
    }

    public class Game
    {
        public IList<Creature> Creatures;
    }

    public class AttackModifier
    {

    }

    public class ChainExerciseTests
    {
        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}