using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns
{
    class Program
    {
        class Journal
        {
            private readonly List<string> entries = new List<string>();

            private static int count = 0;

            public int Add(string entry)
            {
                entries.Add($"{++count}: {entry}");
                return count; // memento
            }

            public void Remove(int index)
            {
                entries.RemoveAt(index);
            }

            public override string ToString()
            {
                return string.Join(Environment.NewLine, entries);
            }

            public void Save(string fileName)
            {
                File.WriteAllText(fileName, ToString());
            }

            public static Journal Load(string filePath)
            {

            }

            public void Load(Uri address)
            {

            }
        }
        
        static void Main(string[] args)
        {
            var j = new Journal();
            j.Add("I cried today");
            j.Add("I ate a bug");
            Console.WriteLine(j);
        }
    }
}
