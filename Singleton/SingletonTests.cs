using Autofac;
using MoreLinq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

    public sealed class PerThreadSingleton
    {
        private static readonly ThreadLocal<PerThreadSingleton> _threadInstance
            = new ThreadLocal<PerThreadSingleton>(
                () => new PerThreadSingleton());

        private PerThreadSingleton()
        {
            Id = Thread.CurrentThread.ManagedThreadId;
        }

        public int Id;

        public static PerThreadSingleton Instance = _threadInstance.Value;
    }

    public sealed class BuildingContext : IDisposable
    {
        private static readonly Stack<BuildingContext> _stack = new Stack<BuildingContext>();

        public int WallHeight;

        static BuildingContext()
        {
            _stack.Push(new BuildingContext(0));
        }

        public BuildingContext(int wallHeight)
        {
            WallHeight = wallHeight;
            _stack.Push(this);
        }

        public static BuildingContext Current => _stack.Peek();

        public void Dispose()
        {
            if (_stack.Count > 1)
            _stack.Pop();
        }
    }

    public class Building
    {
        public List<Wall> Walls = new List<Wall>();

        public override string ToString()
        {
            var sb = new StringBuilder();
           
            foreach (var w in Walls)
            {
                sb.AppendLine(w.ToString());
            }
            
            return sb.ToString();
        }
    }

    public struct Point
    {
        public int X, Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"X = {X} / Y = {Y}";
        }
    }
    public class Wall
    {
        public Point Start, End;
        public int Height;

        public Wall(Point start, Point end)
        {
            Start = start;
            End = end;
            Height = BuildingContext.Current.WallHeight;
        }

        public override string ToString()
        {
            return $"Start = {Start} -- End = {End} with Height = {Height}";
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

        [Test]
        public async Task Test_per_thread_singleton()
        {
            var t1 = Task.Factory.StartNew(() =>
            {
                Debug.WriteLine($"T1: " + PerThreadSingleton.Instance.Id);
            });

            var t2 = Task.Factory.StartNew(() =>
            {
                Debug.WriteLine($"T2: " + PerThreadSingleton.Instance.Id);
                Debug.WriteLine($"T2: " + PerThreadSingleton.Instance.Id);
            });

            await Task.WhenAll(t1, t2);
        }

        [Test]
        public void Test_house()
        {
            var house = new Building();

            // ground floor at 3000
            using (new BuildingContext(3000))
            {
                house.Walls.Add(new Wall(new Point(0, 0), new Point(5000, 0)));
                house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));

                Debug.WriteLine(house);

                // 1st floor at 3500
                using (new BuildingContext(3500))
                {
                    house.Walls.Add(new Wall(new Point(0, 0), new Point(6000, 0)));
                    house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));

                    Debug.WriteLine(house);
                }

                // ground floor at 3000
                house.Walls.Add(new Wall(new Point(5000, 0), new Point(5000, 4000)));

                Debug.WriteLine(house);
            }
        }
    }
}