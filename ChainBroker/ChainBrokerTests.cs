using NUnit.Framework;
using System;
using System.Diagnostics;

namespace ChainBroker
{
    public class Game
    {
        public event EventHandler<Query> Queries;

        public void PerformQuery(object sender, Query q)
        {
            Queries?.Invoke(sender, q);
        }
    }

    public class Query
    {
        public string CreatureName;

        public enum Argument
        {
            Attack,
            Defense
        }

        public Argument WhatToQuery;

        public int Value;

        public Query(string creatureName, Argument whatToQuery, int value)
        {
            CreatureName = creatureName;
            WhatToQuery = whatToQuery;
            Value = value;
        }
    }

    public class Creature
    {
        private Game _game;
        public string Name;
        private int _attack, _defense;

        public Creature(Game game, string name, int attack, int defense)
        {
            _game = game;
            Name = name;
            _attack = attack;
            _defense = defense;
        }

        public int Attack
        {
            get
            {
                var q = new Query(Name, Query.Argument.Attack, _attack);
                _game.PerformQuery(this, q);
                return q.Value;
            }
        }

        public int Defense
        {
            get
            {
                var q = new Query(Name, Query.Argument.Defense, _defense);
                _game.PerformQuery(this, q);
                return q.Value;
            }
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Attack)}: {Attack}, {nameof(Defense)}: {Defense}";
        }
    }

    public abstract class CreatureModifier : IDisposable
    {
        protected Game _game;
        protected Creature _creature;

        public CreatureModifier(Game game, Creature creature)
        {
            _game = game;
            _creature = creature;
            _game.Queries += Handle;
        }

        protected abstract void Handle(object sender, Query q);

        public void Dispose()
        {
            _game.Queries -= Handle;
        }
    }

    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Game game, Creature creature) : base(game, creature) 
        {
                
        }
        protected override void Handle(object sender, Query q)
        {
            if (q.CreatureName == _creature.Name
                && q.WhatToQuery == Query.Argument.Attack)
            {
                q.Value *= 2;
            }
        }
    }

    public class IncreaseDefenseModifier : CreatureModifier
    {
        public IncreaseDefenseModifier(Game game, Creature creature) : base(game, creature)
        {

        }
        protected override void Handle(object sender, Query q)
        {
            if (q.CreatureName == _creature.Name
                && q.WhatToQuery == Query.Argument.Defense)
            {
                q.Value += 3;
            }
        }
    }

    public class ChainBrokerTests
    {
        [Test]
        public void Run_test_for_goblin_modifications()
        {
            var game = new Game();
            var goblin = new Creature(game, "Strong Goblin", 3, 3);

            Debug.WriteLine(goblin);

            using (new DoubleAttackModifier(game, goblin))
            {
                Debug.WriteLine(goblin);
                using (new IncreaseDefenseModifier(game, goblin))
                {
                    Debug.WriteLine(goblin);
                }
            }

            Debug.WriteLine(goblin);
        }
    }
}