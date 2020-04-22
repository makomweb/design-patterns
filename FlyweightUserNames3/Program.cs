using JetBrains.dotMemoryUnit;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyweightUserNames3
{
    public class User
    {
        public string FullName { get; private set; }

        public User(string fullName)
        {
            FullName = fullName;
        }
    }

    [TestFixture]
    class Program
    {
        [DotMemoryUnit(CollectAllocations = true)]
        [Test]
        public void TestUser()
        {
            var firstNames = Enumerable.Range(0, 100)
                .Select(_ => RandomString());

            var lastNames = Enumerable.Range(0, 100)
                .Select(_ => RandomString());

            var users = new List<User>();

            foreach (var firstName in firstNames)
                foreach (var lastName in lastNames)
                    users.Add(new User($"{firstName} {lastName}"));

            ForceGC();

            dotMemory.Check(memory => Console.WriteLine(memory.SizeInBytes));
        }

        private void ForceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private string RandomString()
        {
            var r = new Random();

            return new string(
                Enumerable.Range(0, 10)
                .Select(i => (char)('a' + r.Next(26)))
                .ToArray());
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
