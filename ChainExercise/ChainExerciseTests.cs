using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChainExercise
{
    public abstract class Creature
    {
        protected readonly Game _game;
        protected readonly int _baseAttack;
        protected readonly int _baseDefense;

        public virtual int Attack { get; set; }

        public virtual int Defense { get; set; }

        public Creature(Game game, int baseAttack, int baseDefense)
        {
            _game = game;
            _baseAttack = baseAttack;
            _baseDefense = baseDefense;
        }

        public abstract void Query(object source, StatQuery sq);
    }

    public enum Statistic
    {
        Attack, Defense
    }
    public class StatQuery
    {
        public Statistic Type { get; set; }

        public int Value { get; set; }
    }

    public class Goblin : Creature
    {
        public Goblin(Game game) : base(game, 1, 1)
        {
        }

        protected Goblin(Game game, int baseAttack, int baseDefense) : base(game, baseAttack, baseDefense)
        {
        }

        public override void Query(object source, StatQuery sq)
        {
            if (ReferenceEquals(source, this))
            {
                switch(sq.Type)
                {
                    case Statistic.Attack:
                        sq.Value += _baseAttack;
                        break;
                    case Statistic.Defense:
                        sq.Value += _baseDefense;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"{sq.Type} out of range!");
                }
            }
            else
            {
                if (sq.Type == Statistic.Defense)
                {
                    sq.Value++;
                }
            }
        }

        public override int Defense
        {
            get
            {
                var q = new StatQuery { Type = Statistic.Defense };
                foreach (var c in _game.Creatures)
                    c.Query(this, q);
                return q.Value;
            }
        }

        public override int Attack
        {
            get
            {
                var q = new StatQuery { Type = Statistic.Attack };
                foreach (var c in _game.Creatures)
                    c.Query(this, q);
                return q.Value;
            }
        }
    }

    public class GoblinKing : Goblin
    {
        public GoblinKing(Game game) : base(game, 3, 3)
        {
        }

        public override void Query(object source, StatQuery sq)
        {
            if (!ReferenceEquals(source, this) && sq.Type == Statistic.Attack)
            {
                sq.Value++; // every goblin gets +1 attack
            }
            else base.Query(source, sq);
        }
    }

    public class Game
    {
        public IList<Creature> Creatures = new List<Creature>();
    }

    public class ChainExerciseTests
    {
        [Test]
        public void Assert_goblin()
        {
            var game = new Game();
            var goblin = new Goblin(game); game.Creatures.Add(goblin);
            Assert.AreEqual(1, goblin.Attack);
            Assert.AreEqual(1, goblin.Defense);
        }

        [Test]
        public void Assert_3_ordinary_goblins()
        {
            var game = new Game();
            var g1 = new Goblin(game); game.Creatures.Add(g1);
            var g2 = new Goblin(game); game.Creatures.Add(g2);
            var g3 = new Goblin(game); game.Creatures.Add(g3);

            Assert.True(game.Creatures.All(c => c.Defense == 3));
            Assert.True(game.Creatures.All(c => c.Attack == 1));
        }

        [Test]
        public void Assert_goblin_king()
        {
            var game = new Game();
            var g1 = new Goblin(game); game.Creatures.Add(g1);
            var g2 = new Goblin(game); game.Creatures.Add(g2);
            var g3 = new Goblin(game); game.Creatures.Add(g3);
            var king = new GoblinKing(game); game.Creatures.Add(king);

            Assert.True(game.Creatures.Where(c => !(c is GoblinKing)).All(c => c.Attack == 2 && c.Defense == 4));
            Assert.True(game.Creatures.Where(c => c is GoblinKing).All(c => c.Attack == 3 && c.Defense == 6));
        }
    }
}