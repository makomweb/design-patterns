using MoreLinq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Singleton
{
    public interface IDatabase
    {
        int GetPopulation(string name);
    }

    public class SingletonDatabase : IDatabase
    {
        private readonly Dictionary<string, int> capitals;

        private static readonly Lazy<SingletonDatabase> _instance =
            new Lazy<SingletonDatabase>(() => new SingletonDatabase());

        private static int _instanceCount = 0;

        public static int Count => _instanceCount;

        public static SingletonDatabase GetInstance() { return _instance.Value; }

        private SingletonDatabase()
        {
            _instanceCount++;

            WriteLine("Initializing database.");

            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, @"capitals.txt");

            capitals = File.ReadAllLines(path)
                .Batch(2)
                .ToDictionary(
                list => list.ElementAt(0).Trim(),
                list => int.Parse(list.ElementAt(1))
                );
        }

        public int GetPopulation(string name)
        {
            return capitals[name];
        }
    }

    public class SingletonTests
    {
        [Test]
        public void IsSingletonTest()
        {
            var db = SingletonDatabase.GetInstance();
            var db2 = SingletonDatabase.GetInstance();

            Assert.That(db, Is.SameAs(db2));
            Assert.AreEqual(1, SingletonDatabase.Count);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var db = SingletonDatabase.GetInstance();
            var city = "Tokyo";
            var population= db.GetPopulation(city);

            WriteLine($"{city} has population {population}");
        }
    }
}
