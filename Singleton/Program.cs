using MoreLinq;
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

        private static readonly SingletonDatabase _instance = new SingletonDatabase();

        public static SingletonDatabase GetInstance() { return _instance; }

        private SingletonDatabase()
        {
            WriteLine("Initializing database.");

            capitals = File.ReadAllLines("capitals.txt")
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

    class Program
    {
        static void Main(string[] args)
        {
            var db = SingletonDatabase.GetInstance();


        }
    }
}
