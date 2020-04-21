using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace DecoratorCreature
{
    public interface ICreature
    {
        int Age { get; set; }
    }

    public interface IBird : ICreature 
    {
#if false
        // Note: Interfaces with default implementation only work from C# 8 and above!
        void Fly()
        {
            if (Age => 10)
            {
                WriteLine("I am flying");
            }
        }
#else
        void Fly();
#endif
    }

    public interface ILizard : ICreature
    {
#if false
        void Crawl()
        {
            if (Age < 10)
            {
                WriteLine("I am crawling");
            }
        }
#else
        void Crawl();
#endif
    }

    public class Organism
    {

    }

    public class Dragon : Organism, ICreature // IBird, ILizard // disabled because not running on a C#8 compiler!
    {
        public int Age { get; set; }
    }

    // sevaral options!
    // - inheritance
    // - SmartDragon(Dragon)
    // - extension methods
    // - C#8 default interface methods
    class Program
    {
        static void Main(string[] args)
        {
            var d = new Dragon { Age = 5 };
            if (d is IBird bird)
            {
                bird.Fly();
            }

            if (d is ILizard lizard)
            {
                lizard.Crawl();
            }
        }
    }
}
