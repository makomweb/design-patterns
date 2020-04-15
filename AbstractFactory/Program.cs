using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace AbstractFactory
{
    public interface IHotDrink
    {
        void Consume();
    }

    internal class Tea : IHotDrink
    {
        public void Consume()
        {
            WriteLine("This tea is nice but I'd prefer it with milk.");
        }
    }

    internal class Coffee : IHotDrink
    {
        public void Consume()
        {
            WriteLine("This coffee is sensational!");
        }
    }

    public interface IHotDrinkFactory
    {
        IHotDrink Prepare(int amount);
    }

    internal class TeaFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            WriteLine($"PUt in a tea bag, boil water, pour {amount} ml, add lemon, enjoy!");
            return new Tea();
        }
    }

    internal class CoffeeFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            WriteLine($"Grind beans, boil water, pour {amount} ml, add cream and sugar, enjoy!");
            return new Coffee();
        }
    }

    public class HotDrinkMachine
    {
        public enum AvailableDrink
        {
            Coffee, Tea
        }

        private Dictionary<AvailableDrink, IHotDrinkFactory> factories = new Dictionary<AvailableDrink, IHotDrinkFactory>();

        public HotDrinkMachine()
        {
#if true
            foreach (AvailableDrink drink in Enum.GetValues(typeof(AvailableDrink)))
            {
                var name = Enum.GetName(typeof(AvailableDrink), drink);
                var factory = (IHotDrinkFactory) Activator.CreateInstance(Type.GetType("AbstractFactory." + name));
                factories.Add(drink, factory);
            }
#else
            factories.Add(AvailableDrink.Coffee, new CoffeeFactory());
            factories.Add(AvailableDrink.Tea, new TeaFactory());
#endif
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
