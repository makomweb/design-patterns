using NUnit.Framework;
using System.Diagnostics;

namespace TemplateMethodExercise
{
    [DebuggerDisplay("Attack = {Attack}, Health = {Health}")]
    public class Creature
    {
        public int Attack, Health;

        public Creature(int attack, int health)
        {
            Attack = attack;
            Health = health;
        }
    }

    public abstract class CardGame
    {
        public Creature[] Creatures;

        public CardGame(Creature[] creatures)
        {
            Creatures = creatures;
        }

        // returns -1 if no clear winner (both alive or both dead)
        public int Combat(int creature1, int creature2)
        {
            Creature first = Creatures[creature1];
            Creature second = Creatures[creature2];
            Hit(first, second);
            Hit(second, first);
            bool firstAlive = first.Health > 0;
            bool secondAlive = second.Health > 0;
            if (firstAlive == secondAlive) return -1;
            return firstAlive ? creature1 : creature2;
        }

        // attacker hits other creature
        protected abstract void Hit(Creature attacker, Creature other);
    }

    public class TemporaryCardDamageGame : CardGame
    {
        public TemporaryCardDamageGame(Creature[] creatures) : base(creatures)
        {
        }

        protected override void Hit(Creature attacker, Creature other)
        {
            var oldHealth = other.Health;
            other.Health -= attacker.Attack;
            if (other.Health > 0)
                other.Health = oldHealth;
        }
    }

    public class PermanentCardDamage : CardGame
    {
        public PermanentCardDamage(Creature[] creatures) : base(creatures)
        {
        }

        protected override void Hit(Creature attacker, Creature other)
        {
            other.Health -= attacker.Attack;
        }
    }

    public class Tests
    {
        [Test]
        public void Test1()
        {
            var creatures = new []
            {
                new Creature(11, 100),
                new Creature(22, 100)
            };

            var game = new PermanentCardDamage(creatures);

            int winner = -1;

            do { winner = game.Combat(0, 1); }
            while (winner == -1);

            Assert.AreEqual(1, winner);
        }
    }
}