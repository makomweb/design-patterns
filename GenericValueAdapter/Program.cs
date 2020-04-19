using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericValueAdapter
{
    public class Vector<T, D>
    {
        protected T[] _data;

        public Vector()
        {
            _data = new T[D];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
