using NUnit.Framework;
using System;

namespace DecoratorExercise
{
    public class Bird
    {
        public int Age { get; set; }

        public string Fly()
        {
            return (Age < 10) ? "flying" : "too old";
        }
    }

    public class Lizard
    {
        public int Age { get; set; }

        public string Crawl()
        {
            return (Age > 1) ? "crawling" : "too young";
        }
    }

    public class Dragon // no need for interfaces
    {
        public int Age
        {
            get; set;
        }

        public string Fly()
        {
            return new Bird { Age = Age }.Fly();
        }

        public string Crawl()
        {
            return new Lizard { Age = Age }.Crawl();
        }
    }

    public class DecoratorExerciseTests
    {
        [Test]
        public void Young_dragons_can_fly()
        {
            var d = new Dragon { Age = 9 };
            Assert.AreEqual("flying", d.Fly());
        }

        [Test]
        public void Old_dragons_cant_fly()
        {
            var d = new Dragon { Age = 11 };
            Assert.AreEqual("too old", d.Fly());
        }

        [Test]
        public void Mature_dragons_can_crawl()
        {
            var d = new Dragon { Age = 2 };
            Assert.AreEqual("crawling", d.Crawl());
        }


        [Test]
        public void Baby_dragons_cant_crawl()
        {
            var d = new Dragon { Age = 0 };
            Assert.AreEqual("too young", d.Crawl());
        }
    }
}