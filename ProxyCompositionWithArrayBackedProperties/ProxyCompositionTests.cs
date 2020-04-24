using NUnit.Framework;

namespace ProxyCompositionWithArrayBackedProperties
{
    public class MasonrySettings
    {
        public bool? All
        {
            get
            {
                if (Pillars == Walls &&
                    Walls == Floors)
                {
                    return Pillars;
                }
                return null;
            }
            set
            {
                if (!value.HasValue) return;
                Pillars = value.Value;
                Walls = value.Value;
                Floors = value.Value;
            }
        }

        public bool Pillars, Walls, Floors;
    }

    public class ProxyCompositionTests
    {
        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}