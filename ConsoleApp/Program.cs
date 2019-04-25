using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns
{
    class Program
    {
        private const string FilePath = @"c:\Temp\journal.txt";

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
        }

        class Persistence
        {
            public static void SaveToFile(Journal j, string filePath, bool overwrite = false)
            {
                if (overwrite || !File.Exists(filePath))
                {
                    File.WriteAllText(filePath, j.ToString());
                }
            }

            public static Journal Load(string filePath)
            {
                throw new NotImplementedException();
            }

            public void Load(Uri address)
            {
                throw new NotImplementedException();
            }
        }
        
        static void Main(string[] args)
        {
            var j = new Journal();
            j.Add("I cried today");
            j.Add("I ate a bug");
            Console.WriteLine(j);

            Persistence.SaveToFile(j, FilePath, true);

            Process.Start(FilePath);
        }
    }
}
