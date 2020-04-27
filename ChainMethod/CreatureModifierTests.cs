using NUnit.Framework;
using System;
using System.Diagnostics;

namespace ChainMethod
{
    public class Creature
    {
        public string Name { get; private set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }

        public Creature(string name, int attack, int defense)
        {
            Name = name;
            Attack = attack;
            Defense = defense;
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Attack)}: {Attack}, {nameof(Defense)}: {Defense}";
        }
    }

    public class CreatureModifier
    {
        protected Creature _creature;

        protected CreatureModifier _next; // linked list

        public CreatureModifier(Creature creature)
        {
            _creature = creature ?? throw new ArgumentNullException(paramName: nameof(creature));
        }

        public void Add(CreatureModifier modifier)
        {
            if (_next != null) _next.Add(modifier);
            else _next = modifier;
        }

        public virtual void Handle() => _next?.Handle();
    }

    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Creature creature) : base(creature)
        {

        }

        public override void Handle()
        {
            Debug.WriteLine($"Doubling {_creature.Name}'s attack");
            _creature.Attack *= 2;
            base.Handle();
        }
    }

    public class IncreasedDefenseModidifer : CreatureModifier
    {
        public IncreasedDefenseModidifer(Creature creature) : base(creature)
        {

        }

        public override void Handle()
        {
            Debug.WriteLine($"Increasing {_creature.Name}'s defense");
            _creature.Defense += 3;
            base.Handle();
        }
    }

    public class NoBonusesModifier : CreatureModifier
    {
        public NoBonusesModifier(Creature creature) :base(creature)
        {
        }

        public override void Handle()
        {
            // Do nothing!
        }
    }

    public class CreatureModifierTests
    {
        [Test]
        public void Test_goblin_attack_if_no_bonuses_are_allowed()
        {
            var goblin = new Creature("Goblin", 2, 2);
            var root = new CreatureModifier(goblin);

            root.Add(new NoBonusesModifier(goblin));

            Debug.WriteLine("Let's double the goblin's attack");

            root.Add(new DoubleAttackModifier(goblin));

            root.Handle();

            Debug.WriteLine(goblin);
        }

        [Test]
        public void Test_goblin_attack_defense()
        {
            var goblin = new Creature("Goblin", 2, 2);
            var root = new CreatureModifier(goblin);

            Debug.WriteLine("Let's double the goblin's attack");
            root.Add(new DoubleAttackModifier(goblin));

            Debug.WriteLine("Let's increase the goblin's defense");
            root.Add(new IncreasedDefenseModidifer(goblin));
            root.Handle();

            Debug.WriteLine(goblin);
        }
    }
}