using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace IteratorArrayBackedProperties
{
    public class Creature : IEnumerable<int>
    {
#if true
        private int[] stats = new int[3];
        const int strength = 0;

        public int Strength
        {
            get { return stats[strength]; }
            set { stats[strength] = value; }
        }
        public int Agility
        {
            get { return stats[1]; }
            set { stats[1] = value; }
        }
        public int Intelligence
        {
            get { return stats[2]; }
            set { stats[2] = value; }
        }

        public double AverageStat => stats.Average();

        public IEnumerator<int> GetEnumerator()
        {
            return stats.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int this [int index]
        {
            get { return stats[index]; }
            set { stats[index] = value; }
        }
#else
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }

        public double AverageStat
        {
            get { return (Strength + Agility + Intelligence) / 3.0; }
        }
#endif
    }

    public class IteratorArrayBackePropertiesTests
    {
        [Test]
        public void Creature_should_have_3_properties()
        {
            var c = new Creature { Agility = 1, Strength = 3, Intelligence = 11 };

            Assert.AreEqual(3, c.Count());
        }
    }
}