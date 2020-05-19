using System;
using System.Threading;
using System.Threading.Tasks;

namespace SingletonPerThread
{
    public sealed class PerThreadSingleton
    {
        private static ThreadLocal<PerThreadSingleton> _threadInstance
            = new ThreadLocal<PerThreadSingleton>(
                () => new PerThreadSingleton(), false);

        private PerThreadSingleton()
        {
            Id = Thread.CurrentThread.ManagedThreadId;
        }

        public int Id;

        public static PerThreadSingleton Instance = _threadInstance.Value;
    }

    class Program
    {
        static void Main(string[] args)
        {
            var t1 = Task.Factory.StartNew(() =>
            {
                //var id = Thread.CurrentThread.ManagedThreadId;
                var id = PerThreadSingleton.Instance.Id;
                Console.WriteLine($"t1: " +  id);
            });

            var t2 = Task.Factory.StartNew(() =>
            {
                //var id = Thread.CurrentThread.ManagedThreadId;
                var id = PerThreadSingleton.Instance.Id;
                Console.WriteLine($"t2: " + id);
            });

           Task.WaitAll(t1, t2);
        }
    }
}
