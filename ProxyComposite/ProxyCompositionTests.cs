using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.ComTypes;

namespace ProxyComposite
{
    public class Creature
    {
        public byte Age;
        public int X, Y;
    }

    public class Creatures
    {
        private readonly int _size;
        private byte[] _age;
        private int[] x, y;

        public Creatures(int size)
        {
            _size = size;
            _age = new byte[size];
            x = new int[size];
            y = new int[size];
        }

        public struct CreatureProxy
        {
            private Creatures _creatures;
            private int _index;

            public CreatureProxy(Creatures creatures, int index)
            {
                _creatures = creatures;
                _index = index;
            }

            public ref byte Age => ref _creatures._age[_index];
            public ref int X => ref _creatures.x[_index];
            public ref int Y => ref _creatures.y[_index];
        }

        public IEnumerator<CreatureProxy> GetEnumerator()
        {
            for (int pos = 0; pos < _size; ++pos)
            {
                yield return new CreatureProxy(this, pos);
            }
        }
    }

    // AOS / SOA duality
    public class ProxyCompositionTests
    {
        [Test]
        public void Move_creatures()
        {
            const int LENGTH = 100;

            // Array of structures (AoS)
            var creatures = new Creature[LENGTH];

            for (var i = 0; i < LENGTH; i++)
            {
                creatures[i] = new Creature();
            }

            foreach (var c in creatures)
            {
                c.X++;
            }
        }

        [Test]
        public void Move_creatures_improved()
        {
            // Structure of Arrays (SoA)
            var creatures = new Creatures(100);
            foreach (var c in creatures)
            {
                c.X++;
            }
        }
    }
}