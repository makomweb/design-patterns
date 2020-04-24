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
    public class ProxyCompositionTests
    {
        private Creature[] _create = new Creature[100];

        [Test]
        public void Move_creatures()
        {
            foreach (var c in _create)
            {
                c.X++;
            }
        }
    }
}
