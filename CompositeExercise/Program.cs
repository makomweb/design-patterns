using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeExercise
{
    public interface IValueContainer : IEnumerable<int>
    {

    }

    public class SingleValue : IValueContainer
    {
        public int Value;

        public IEnumerator<int> GetEnumerator()
        {
            yield return Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ManyValues : List<int>, IValueContainer
    {

    }

    public static class ExtensionMethods
    {
        public static int Sum(this List<IValueContainer> containers)
        {
            int result = 0;
            foreach (var c in containers)
                foreach (var i in c)
                    result += i;
            return result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            {
                var value = new SingleValue { Value = 55 };
                var sum = ExtensionMethods.Sum(new List<IValueContainer> { value });
                Debug.Assert(55 == sum);
            }

            {
                var values = new ManyValues { 22, 33, 44 };
                var sum = ExtensionMethods.Sum(new List<IValueContainer> { values });
                Debug.Assert(99 == sum);
            }
        }
    }
}
