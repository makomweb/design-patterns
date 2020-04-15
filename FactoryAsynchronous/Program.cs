using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactoryAsynchronous
{
    public class Foo
    {
        public async Task<Foo> InitAsync()
        {
            await Task.Delay(100);
            return this;
        }

        private Foo()
        {

        }

        public static Task<Foo> CreateAsync()
        {
            var foo = new Foo();
            return foo.InitAsync();
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            //var foo = new Foo();
            //await foo.InitAsync();

            var x = await Foo.CreateAsync();
        }
    }
}
