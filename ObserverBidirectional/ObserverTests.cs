using NUnit.Framework;

namespace ObserverBidirectional
{
    public class Product
    {
        public string Name { get; set; }
    }

    public class Window
    {
        public string ProductName { get; set; }
    }

    public class ObserverTests
    {
        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}