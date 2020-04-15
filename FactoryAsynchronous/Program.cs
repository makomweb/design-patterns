using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryAsynchronous
{
    public class Foo
    {
        public Task InitAsync()
        {
            return Task.Delay(100);
        }

        public Foo()
        {

        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var foo = new Foo();
            await foo.InitAsync();
        }
    }
}
