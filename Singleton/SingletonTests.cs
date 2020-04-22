using Autofac;
using MoreLinq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Console;

namespace Singleton
{
    public interface IDatabase
    {
        int GetPopulation(string name);
    }

    public class SingletonDatabase : IDatabase
    {
        private readonly Dictionary<string, int> _capitals;

        private static readonly Lazy<SingletonDatabase> _instance =
            new Lazy<SingletonDatabase>(() => new SingletonDatabase());

        public static int Count { get; set; } = 0;

        public static SingletonDatabase GetInstance() { return _instance.Value; }

        private SingletonDatabase()
        {
            Count++;

            WriteLine("Initializing database.");

            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, @"capitals.txt");

            _capitals = File.ReadAllLines(path)
                .Batch(2)
                .ToDictionary(
                list => list.ElementAt(0).Trim(),
                list => int.Parse(list.ElementAt(1))
                );
        }

        public int GetPopulation(string name)
        {
            return _capitals[name];
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

    public class ConfigurableRecordFinder
    {
        private IDatabase _database;

        public ConfigurableRecordFinder(IDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(paramName: nameof(database));
        }

        public int GetTotalPopulation(params string[] names)
        {
            int result = 0;
            foreach (var name in names)
            {
                result += _database.GetPopulation(name);

            }
            return result;
        }
    }

    public class DummyDatabase : IDatabase
    {
        public int GetPopulation(string name)
        {
            return new Dictionary<string, int>
            {
                ["alpha"] = 1,
                ["beta"] = 2,
                ["gamma"] = 3
            }[name];
        }
    }

    public class OrdinaryDatabase : IDatabase
    {
        private readonly Dictionary<string, int> _capitals;

        public OrdinaryDatabase()
        {
            WriteLine("Initializing database.");

            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, @"capitals.txt");

            _capitals = File.ReadAllLines(path)
                .Batch(2)
                .ToDictionary(
                list => list.ElementAt(0).Trim(),
                list => int.Parse(list.ElementAt(1))
                );
        }

        public int GetPopulation(string name)
        {
            return _capitals[name];
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

        [Test]
        public void ConfigurablePopulationTest()
        {
            var rf = new ConfigurableRecordFinder(new DummyDatabase());
            var names = new[] { "alpha", "gamma" };
            int tp = rf.GetTotalPopulation(names);
            Assert.AreEqual(4, tp);
        }

        [Test]
        public void DIPopulationTest()
        {
            var cb = new ContainerBuilder();
            cb.RegisterType<OrdinaryDatabase>()
                .As<IDatabase>()
                .SingleInstance();
            cb.RegisterType<ConfigurableRecordFinder>();

            using (var c = cb.Build())
            {
                var rf = c.Resolve<ConfigurableRecordFinder>();

                var population = rf.GetTotalPopulation("New York");

                Assert.AreEqual(17800000, population);
            }
        }

        [Test]
        public void TestewYorkPopulation()
        {
            var db = SingletonDatabase.GetInstance();
            var population = db.GetPopulation("New York");

            Assert.AreEqual(17800000, population);
        }

        [Test]
        public void TestTokyoPopulation()
        {
            var db = SingletonDatabase.GetInstance();
            var population = db.GetPopulation("﻿Tokyo");

            Assert.AreEqual(33200000, population);
        }
    }
}