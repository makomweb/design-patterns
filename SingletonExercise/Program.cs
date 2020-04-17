using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingletonExercise
{
    public class SingletonTester
    {
        public static bool IsSingleton(Func<object> func)
        {
            var obj1 = func();
            var obj2 = func();
            return obj1 == obj2;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
