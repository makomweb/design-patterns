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
    }

    public class Bird : IBird
    {
        public void Fly()
        {
            WriteLine("Soaring in the sky");
        }
    }

    public interface ILizard
    {
        void Crawl();
    }

    public class Lizard : ILizard
    {
        public void Crawl()
        {
            WriteLine("Crawling in the dirt");
        }
    }

    public class Dragon : IBird, ILizard
    {
        public Bird _bird = new Bird();
        public Lizard _lizard = new Lizard();

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
        }
    }
}
