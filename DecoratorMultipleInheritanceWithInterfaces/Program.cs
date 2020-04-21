using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace DecoratorMultipleInheritanceWithInterfaces
{
    public interface IBird
    {
        void Fly();
        int Weight { get; set; }
    }

    public class Bird : IBird
    {
        public int Weight { get; set; }

        public void Fly()
        {
            WriteLine($"Soaring in the sky with weight {Weight}");
        }
    }

    public interface ILizard
    {
        void Crawl();
        int Weight { get; set; }
    }

    public class Lizard : ILizard
    {
        public int Weight { get; set; }

        public void Crawl()
        {
            WriteLine($"Crawling in the dirt with weight {Weight}");
        }
    }

    public class Dragon : IBird, ILizard
    {
        public Bird _bird = new Bird();
        public Lizard _lizard = new Lizard();

        public int Weight { get; set; }

        public void Crawl()
        {
            _lizard.Crawl();
        }

        public void Fly()
        {
            _bird.Fly();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var d = new Dragon();
            d.Weight = 123;
            d.Fly();
            d.Crawl();
        }
    }
}
