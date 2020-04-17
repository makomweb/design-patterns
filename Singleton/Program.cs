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

        public static int Count { get; set; } = 0;

        public static SingletonDatabase GetInstance() { return _instance.Value; }

        private SingletonDatabase()
        {
            Count++;

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

    public class SingletonRecordFinder
    {
        public int GetTotalPopulation(IEnumerable<string> names)
        {
            int result = 0;
            foreach (var name in names)
            {
                result += SingletonDatabase.GetInstance().GetPopulation(name);

            }
            return result;
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

        [Test]
        public void SingletonTotalPopulationTest()
        {
            var rf = new SingletonRecordFinder();
            var names = new[] { "Seoul", "Mexico City" };
            int tp = rf.GetTotalPopulation(names);
            Assert.AreEqual(17500000 + 17400000, tp);
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
