using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChainExercise
{
    public abstract class Creature
    {
        public int Attack { get; set; }
        public int Defense { get; set; }
    }

    public class Goblin : Creature
    {
        public Goblin(Game game)
        {
            game.Creatures.Add(this);

            new AttributeModifier(this, game).Handle();
        }
    }

    public class GoblinKing : Goblin
    {
        public GoblinKing(Game game) : base(game)
        {
        }
    }

    public class Game
    {
        public IList<Creature> Creatures = new List<Creature>();
    }

    public class AttributeModifier
    {
        private Creature _creature;
        private Game _game;

        public AttributeModifier(Creature creature, Game game)
        {
            _creature = creature;
            _game = game;
        }

        public void Handle()
        {
            {
                var newDefense = _game.Creatures.Count;
                foreach (var c in _game.Creatures)
                {
                    if (c is GoblinKing)
                    {
                        c.Defense = newDefense + 2;
                    }
                    else
                    {
                        c.Defense = newDefense;
                    }
                }
            }

            {
                if (_game.Creatures.Any(c => c is GoblinKing))
                {
                    foreach (var c in _game.Creatures)
                    {
                        if (c is GoblinKing)
                        {
                            c.Attack = 3;
                        }
                        else
                        {
                            c.Attack = 2;
                        }
                    }

                }
                else
                {
                    foreach (var c in _game.Creatures)
                    {
                        c.Attack = 1;
                    }
                }
            }
        }
    }

    public class ChainExerciseTests
    {
        [Test]
        public void Assert_3_ordinary_goblins()
        {
            var game = new Game();
            var g1 = new Goblin(game);
            var g2 = new Goblin(game);
            var g3 = new Goblin(game);

            Assert.True(game.Creatures.All(c => c.Defense == 3));
            Assert.True(game.Creatures.All(c => c.Attack == 1));
        }

        [Test]
        public void Assert_goblin_king()
        {
            var game = new Game();
            var g1 = new Goblin(game);
            var g2 = new Goblin(game);
            var g3 = new Goblin(game);
            var king = new GoblinKing(game);

            Assert.True(game.Creatures.Where(c => !(c is GoblinKing)).All(c => c.Attack == 2 && c.Defense == 4));
            Assert.True(game.Creatures.Where(c => c is GoblinKing).All(c => c.Attack == 3 && c.Defense == 6));
        }
    }
}